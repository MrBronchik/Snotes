using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is to store info about play process & to sum up the results & to show result UI
public class Concluder : MonoBehaviour
{
    public GeneralStats stats = new GeneralStats();

    [Header ("TextFields")]
    [SerializeField] Image lvlLogo;
    [SerializeField] Text lvlName;
    [SerializeField] Text overallHitT;
    [SerializeField] Text accuracyT;
    [SerializeField] Text maxComboT;
    [SerializeField] Text clickAccuracyT;
    [SerializeField] GameObject resultUI;
    [SerializeField] GameProcess gameProcess;

#region Info for result UI

    public GeneralStats CalcResults() {
        stats.accuracy = GetAccuracy();
        return stats;
    }

    private float GetAccuracy(){
        float sumOfAll = 0;
        foreach (float value in gameProcess.points)
        {
            sumOfAll += value;
        }
        return (sumOfAll / stats.numOfNotes);
    }

    public void GetResults() {

        if (stats.maxCombo < stats.combo) stats.maxCombo = stats.combo;
        resultUI.SetActive(true);

        GameObject dataStorer = GameObject.Find("DataStorer");
        Debug.Log(dataStorer);

        lvlLogo.sprite = dataStorer.transform.GetChild(0).GetComponent<Image>().sprite;
        lvlName.text = PlayerPrefs.GetString("lvl name");

        overallHitT.text = string.Format("Total hits: {0} / {1}", gameProcess.points.Count, stats.numOfNotes);
        accuracyT.text = string.Format("Accuracy of hits: {0:P2}", CalcResults().accuracy);
        maxComboT.text = string.Format("Max combo: {0}", stats.maxCombo);
        clickAccuracyT.text = string.Format("Accuracy of clicks: {0:P2}", (float) gameProcess.points.Count / Mathf.Max(stats.clicksNum, stats.numOfNotes));
    }
    
#endregion
}