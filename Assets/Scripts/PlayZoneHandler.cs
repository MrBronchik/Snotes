using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Linq;

public class PlayZoneHandler : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] GameObject parentObj;                                                                          // GO for notes, frozes
    [SerializeField] Sprite[] sprites;                                                                              // note sprites
    [SerializeField] GameObject mainGO;                                                                             // idk                                          // not used
    [SerializeField] Text startText;                                                                                // text-box with start-revealed name of the level
    [SerializeField] public float secondsToWait;                                                                    // number of seconds that the name of level will stay on the screen

    [Header("Scripts")]
    [SerializeField] CameraMovement cameraMovement;                                                                 // script camera
    [SerializeField] AudioHandler audioHandler;                                                                     // script of audio
    [SerializeField] Detector1 detector1;                                                                           // script of detection
    [SerializeField] Concluder concluder;                                                                           // script of resulting

    private float versionOfMusic;                                                                                   // version of schedule file                     // not used
    private float BPM;                                                                                              // BPM of music                                 // not used
    public float delayInSecs;                                                                                       // delay when level starts
    public float speed;                                                                                             // speed of note's flow

    List<GameObject> NotesList;                                                                                     // list with Note game objects
    List<float> timeSchedule;                                                                                       // idk                                          // not used
    private string[] lvlSchedule;                                                                                   // file with schedule by rows
    private string txtPath;                                                                                         // path to lvl schedule
    private string musicPath;                                                                                       // path to music file
    private int difficultyLevel;                                                                                    // difficulty of level                          // not used
    private int numberOfNotes;                                                                                      // number of notes
    private int numberOfFrozes;                                                                                     // number of frozes
    private int linesToSkip;                                                                                        // for file reading             USEFUL

    private void Start() {
        NotesList = new List<GameObject>();
        //timeSchedule = new List<float>();

        // Reading paths from selected level
        txtPath = PlayerPrefs.GetString("txt path");
        musicPath = PlayerPrefs.GetString("music path");

        // Load music
        audioHandler.LoadMusic(musicPath);
        
        // Load schedule
        lvlSchedule = File.ReadAllLines(txtPath);
        Play();
    }

    public void Play() {

        if(!File.Exists(txtPath)) {
            Debug.Log("File do not exists!");
            return;
        }

        // Loading of main information
        versionOfMusic = float.Parse(lvlSchedule[1]);
        BPM = float.Parse(lvlSchedule[2]);
        delayInSecs = float.Parse(lvlSchedule[3]);
        speed = float.Parse(lvlSchedule[4]);
        detector1.SetDetectorThickness(float.Parse(lvlSchedule[5]));

        // Spawn END of the level
            GameObject lvlEnder = new GameObject("END");
            lvlEnder.transform.parent = parentObj.transform;
            lvlEnder.transform.position = new Vector3(float.Parse(lvlSchedule[6]) * speed + detector1.detectionThickness + 16, 0, 0);
            lvlEnder.transform.localScale = new Vector3(1, 1, 1);
            lvlEnder.AddComponent<BoxCollider2D>().size = new Vector2(0.001f, 0.001f);
        //

        // Revealing of level's name
        StartCoroutine(StartRevealOfLevelName());

        // Loading more information
        linesToSkip = 8;
        numberOfNotes = int.Parse(lvlSchedule[linesToSkip]);
        concluder.stats.numOfNotes = numberOfNotes;
        linesToSkip += 2;

        // Generate notes
        SpawnNotes();

        // Again information
        linesToSkip = linesToSkip + numberOfNotes*5;
        numberOfFrozes = int.Parse(lvlSchedule[linesToSkip]);
        linesToSkip += 2;

        // Generate frozes
        SpawnFrozes();        
    }

    private void SpawnNotes() {
        float firstNoteSpawnTime;
        float presentNoteSpawnTime;
        int meanwhile;

        // This cycle calculates how many notes must be played in the same time
        // If so are, they are placed above each other
        for (int i = 0; i < numberOfNotes; i++) {

            // Memorises the time of "first" note
            firstNoteSpawnTime = float.Parse(lvlSchedule[i*5 + linesToSkip + 1]);
            meanwhile = 1;

            i++;

            // Checks index error
            if (i == numberOfNotes) {
                i--;
                CreateNote(i, float.Parse(lvlSchedule[i*5 + linesToSkip + 1]) * speed, 0);
                break;
            }

            // Checks time of next note
            presentNoteSpawnTime = float.Parse(lvlSchedule[i*5 + linesToSkip + 1]);

            // Cycle for calculating the number of notes which has to be spawned on each other
            while (firstNoteSpawnTime == presentNoteSpawnTime) {
                i++;
                meanwhile++;

                // Checks index error
                if (i == numberOfNotes) {
                    Debug.Log("break");
                    break;
                }

                // Checks time again for next note
                presentNoteSpawnTime = float.Parse(lvlSchedule[i*5 + linesToSkip + 1]);
            }

            // The lowest position of note
            float lowestY = -10*meanwhile + 10;

            int row = 0;
            int j = i - meanwhile;
            for (; j < i; j++)
            {
                // Spawn the note on right position
                CreateNote(j, (float.Parse(lvlSchedule[j*5 + linesToSkip + 1]) * speed), lowestY + 20*row);
                row++;
            }
            // returing to previous note, because we had to rise 'i' for checking the time of NEXT note
            i = j - 1;
        }

        Debug.Log("File successfully imported");
    }

    // Note spawner
    private void CreateNote(int koef, float xCoords, float yCoords) {

        GameObject noteToSpawn = new GameObject("N");
        noteToSpawn.transform.parent = parentObj.transform;
        noteToSpawn.transform.position = new Vector3(xCoords, yCoords, 1);
        noteToSpawn.transform.localScale = new Vector3(2, 2, 2);
        
        noteToSpawn.AddComponent<SpriteRenderer>().sprite = sprites[int.Parse(lvlSchedule[koef*5 + linesToSkip + 2])];

        noteToSpawn.AddComponent<BoxCollider2D>().size = new Vector2(0.001f, 0.001f);

        // Gives id (color) to every note, for future use
        string Arg = lvlSchedule[koef*5 + linesToSkip + 2];
        GameObject childNoteToSpawn = new GameObject(Arg);
        childNoteToSpawn.transform.parent = noteToSpawn.transform;

        // Filling list with notes
        NotesList.Add(noteToSpawn);
    }

    // Froze spawner
    void SpawnFrozes() {
        for (int koef = 0; koef < numberOfFrozes; koef++)
        {
            GameObject frozeToSpawn = new GameObject("F");
            frozeToSpawn.transform.parent = parentObj.transform;
            frozeToSpawn.transform.position = new Vector3(float.Parse(lvlSchedule[koef*5 + linesToSkip + 1]) * speed + 16f, 0, 1);
            frozeToSpawn.transform.localScale = new Vector3(2,2,2);

            frozeToSpawn.AddComponent<BoxCollider2D>().size = new Vector2(0.001f, 0.001f);

            // Gives id (color) to every froze, for future use
            string Arg = lvlSchedule[koef*5 + linesToSkip + 2];
            GameObject childFrozeToSpawn = new GameObject(Arg);
            childFrozeToSpawn.transform.parent = frozeToSpawn.transform;
        }
    }

    // Revealing a name of level on the beginning
    IEnumerator StartRevealOfLevelName() {

        Camera camera_ = Camera.main;
        float halfHeight_ = camera_.orthographicSize;
        float halfWidth_ = camera_.aspect * halfHeight_;
        float needsToWaitAdditionly_ = halfWidth_ / speed;

        if (delayInSecs < 0) {
            cameraMovement.transform.position = new Vector3(-halfWidth_ + (delayInSecs * speed), 0, 0);
        } else {
            cameraMovement.transform.position = new Vector3(-halfWidth_, 0, 0); 
        }

        startText.text = Path.GetFileNameWithoutExtension(musicPath);
        yield return new WaitForSeconds(secondsToWait);
        startText.gameObject.SetActive(false);

        cameraMovement.Play();
        
        // Set time when music starts to play
        if (delayInSecs < 0) {
            yield return new WaitForSeconds(needsToWaitAdditionly_);
        } else {
            yield return new WaitForSeconds(delayInSecs + needsToWaitAdditionly_);
        }
        // Play music
        audioHandler.audioSource.Play();
    }
}