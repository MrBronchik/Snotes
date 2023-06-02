using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [Header("Result fields")]
    [SerializeField] Image m_lvlLogo;
    [SerializeField] Text m_lvlName;
    [SerializeField] Text overallHitT;
    [SerializeField] Text accuracyT;
    [SerializeField] Text maxComboT;
    [SerializeField] Text clickAccuracyT;
    [SerializeField] GameObject resultUI;
    //[SerializeField] GameProcess gameProcess;

    private GeneralStats stats = new GeneralStats();

    private void Start()
    {
        stats.combo = 0;
        stats.maxCombo = 0;
        stats.sumOfDistances = 0;
        stats.skippedNotes = 0;
        stats.noteHit = 0;
        stats.clicksNum = 0;
        stats.numOfNotes = 0;
    }
    public void Hit(float detXPos, float notePosX, float detectorThickness)
    {
        // value = how far is note from the center when it was caught
        float value = Mathf.Abs(detXPos - notePosX) * 2 / detectorThickness;

        stats.sumOfDistances += value;

        float p;
        switch (value)
        {
            case var _ when (value <= 0.1f):
                p = 1f;
                break;
            case var _ when (value > 0.1f && value <= 0.2f):
                p = 0.95f;
                break;
            case var _ when (value > 0.2f && value <= 0.3f):
                p = 0.875f;
                break;
            case var _ when (value > 0.3f && value <= 0.45f):
                p = 0.75f;
                break;
            case var _ when (value > 0.45f && value <= 0.6f):
                p = 0.6f;
                break;
            case var _ when (value > 0.6f && value <= 0.8f):
                p = 0.4f;
                break;
            default:
                p = 0.15f;
                break;
        }
        //points.Add(p);
        Debug.Log(p);
        stats.combo++;
        stats.clicksNum++;
        stats.noteHit++;
        stats.numOfNotes++;

        //StartCoroutine(ShowPoints(p, notePosX, 20));
        //SetPulsorColorid(id);
    }

    public void Miss()
    {
        stats.clicksNum++;
        if (stats.maxCombo < stats.combo) stats.maxCombo = stats.combo;
        stats.combo = 0;
    }

    public void Skip()
    {
        stats.skippedNotes++;
        if (stats.maxCombo < stats.combo) stats.maxCombo = stats.combo;
        stats.combo = 0;
        stats.numOfNotes++;
    }

    public void ShowResults()
    {
        if (stats.maxCombo < stats.combo) stats.maxCombo = stats.combo;
        resultUI.SetActive(true);

        GameObject dataStorer = GameObject.Find("DataStorer");
        Debug.Log(dataStorer);

        m_lvlLogo.sprite = dataStorer.transform.GetChild(0).GetComponent<Image>().sprite;
        m_lvlName.text = PlayerPrefs.GetString("lvl name");

        overallHitT.text = string.Format("Total hits: {0} / {1}", stats.noteHit, stats.numOfNotes);
        accuracyT.text = string.Format("Accuracy of hits: {0:P2}", 1.0f - (stats.sumOfDistances / stats.numOfNotes));
        maxComboT.text = string.Format("Max combo: {0}", stats.maxCombo);
        clickAccuracyT.text = string.Format("Accuracy of clicks: {0:P2}", 1.0f - (stats.sumOfDistances / stats.clicksNum));
    }
}
