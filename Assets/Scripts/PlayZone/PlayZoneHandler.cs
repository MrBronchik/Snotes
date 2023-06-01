using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Linq;
using UnityEditor;

public class PlayZoneHandler : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] public GameObject m_objectStorer;                                                                     // GO for notes, frozes
    [SerializeField] Text startText;                                                                                // text-box with start-revealed name of the level
    [SerializeField] public float secondsToWait;                                                                    // number of seconds that the name of level will stay on the screen
    [SerializeField] GameObject m_playLineCamera;                                                                 // camera

    [Header("PreFabs")]
    [SerializeField] GameObject[] m_notePrefabs;
    [SerializeField] GameObject[] m_frozePrefabs;
    [SerializeField] GameObject m_endPrefab;

    [Header("Scripts")]
    [SerializeField] AudioHandler audioHandler;                                                                     // script of audio
    [SerializeField] DetectionHandler detectionHandler;                                                                           // script of detection
    [SerializeField] DataHandler dataHandler;
    [SerializeField] ButtonHandler buttonHandler;
    [SerializeField] Concluder concluder;                                                                           // script of resulting

    private float m_versionOfMusic;                                                                                   // version of schedule file                     // not used
    //private float m_bpm;                                                                                              // BPM of music                                 // not used
    [System.NonSerialized] public float m_delay_secs;                                                                                        // delay when level starts
    private float m_camSpeed;
    private float m_detectorThickness;
    private float m_lvlLength_secs;
    private float m_scrWidth;

    public List<GameObject> detectedNotes;

    public bool levelIsCompleted = false;


    private void Awake()
    {
        m_scrWidth = 2 * m_playLineCamera.GetComponent<Camera>().aspect * m_playLineCamera.GetComponent<Camera>().orthographicSize;

        // Load schedule
        SetUpLevel();

        StartCoroutine(MovingPlayLineCamera());

        detectionHandler.PassReference(this);
        buttonHandler.PassReference(this);
    }

    private void SetUpLevel()
    {
        // Load music
        audioHandler.LoadMusic(PlayerPrefs.GetString("music path"));
        Debug.Log(PlayerPrefs.GetString("music path"));

        // Loads Main level data
        StreamReader file = new StreamReader(PlayerPrefs.GetString("txt path"));

        string line;

        if ((line = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); }
        // m_versionOfMusic = float.Parse(line);

        // Set BPM
        if ((line = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); }
        // m_bpm = float.Parse(line);

        // Set delay
        if ((line = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); }
        m_delay_secs = float.Parse(line);

        // Set speed
        if ((line = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); }
        m_camSpeed = float.Parse(line);
        m_camSpeed *= m_scrWidth;  // Multiplying with width as everything should be relative to the screen

        // Set detector thickness
        if ((line = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); }
        // m_detectorThickness = float.Parse(line);
        m_detectorThickness = 0.5f;
        detectionHandler.SetDetectorThickness(m_detectorThickness, m_scrWidth);

        // Set level length
        if ((line = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); }

        m_lvlLength_secs = float.Parse(line);

        // Loads all the aobjects from lvl file
        dataHandler.ImportData(file, this);

        // Sets End object
        GameObject go_end = Instantiate(m_endPrefab);
        go_end.transform.position = new Vector3(
               m_lvlLength_secs * m_camSpeed + m_scrWidth / 2,
               0.0f,
               0.0f
               );

        go_end.transform.parent = m_objectStorer.transform;
        
        // Sets camera's location 
        if (m_delay_secs < 0)   // Music starts before the level
        {
            m_playLineCamera.transform.position = new Vector3(
                (m_delay_secs - secondsToWait) * m_camSpeed,
                m_playLineCamera.transform.position.y,
                m_playLineCamera.transform.position.z
                );

            StartCoroutine(audioHandler.PlayIn(m_delay_secs + secondsToWait));
        }
        else
        {
            m_playLineCamera.transform.position = new Vector3(
                -secondsToWait * m_camSpeed,
                m_playLineCamera.transform.position.y,
                m_playLineCamera.transform.position.z
                );
            StartCoroutine(audioHandler.PlayIn(secondsToWait));
        }

    }

    public void PlaceNote(int id, float time_secs)
    {
        GameObject go_note = Instantiate(m_notePrefabs[id]);

        go_note.transform.position = new Vector3(
               time_secs * m_camSpeed,
               0.0f,
               0.0f
               );
        go_note.transform.parent = m_objectStorer.transform;
    }
    public void PlaceFroze(int id, float time_secs)
    {
        GameObject go_froze = Instantiate(m_frozePrefabs[id]);

        go_froze.transform.position = new Vector3(
               time_secs * m_camSpeed,
               0.0f,
               0.0f
               );
        go_froze.transform.parent = m_objectStorer.transform;
    }

    // Revealing a name of level on the beginning
    IEnumerator StartRevealOfLevelName(string levelName)
    {
        startText.text = levelName;
        yield return new WaitForSeconds(secondsToWait);
        startText.gameObject.SetActive(false);
    }

    public void LevelCompleted()
    {
        levelIsCompleted = true;
    }

    IEnumerator MovingPlayLineCamera()
    {
        while(true)
        {
            m_playLineCamera.transform.position = new Vector3(
                m_playLineCamera.transform.position.x + Time.deltaTime * m_camSpeed,
                m_playLineCamera.transform.position.y,
                m_playLineCamera.transform.position.z
                );

            yield return null;
        }
    }

    public void FreezeButton(int index)
    {
        buttonHandler.FreezeButton(index);
    }

    public void ButtonPressed(int index)
    {
        string crucialTag;
        switch (index)
        {
            case 0:
                crucialTag = "RNote";
                break;

            case 1:
                crucialTag = "GNote";
                break;

            case 2:
                crucialTag = "BNote";
                break;

            case 3:
                crucialTag = "ONote";
                break;

            default:
                crucialTag = "";
                break;
        }
        foreach(GameObject note in detectedNotes)
        {
            if (crucialTag == note.tag)
            {
                detectedNotes.Remove(note);
                // Call for success
                note.SetActive(false);
                break;
            }
        }

        // Call for miss
    }
}