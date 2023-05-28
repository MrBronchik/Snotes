using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProcess : MonoBehaviour
{
    public GeneralStats stats = new GeneralStats();

    public List<float> points = new List<float>();

    [SerializeField] GameObject scoresShowerPrototype;
    [SerializeField] float secondsWhenScoresAreShown;
    [SerializeField] float howFarScoresFlyOut;
    [SerializeField] float heightOfScoresAppearing;
    [SerializeField] SpriteRenderer pulsor;
    [SerializeField] Color[] noteColors;
    [SerializeField] float coefOfSpeedToDecreasePulsorsAlpha;

    private void Start() {
        stats.clicksNum = 0;
        stats.skippedNotes = 0;
        stats.maxCombo = 0;
        stats.combo = 0;

        StartCoroutine(SetUpPulsor());
    }

    // Calculates the number of points received
    public void Hit(float detXPos, float notePosX, float detThicknessModified /* Modified means already devided by two */, int id) { 

        // value = how far is note from the center when it was caught
        float value = Mathf.Abs(detXPos - notePosX) / detThicknessModified;

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
        points.Add(p);
        stats.combo++;
        stats.clicksNum++;

        StartCoroutine(ShowPoints(p, notePosX, 20));
        SetPulsorColorid(id);
    }

    public void Miss() {
        stats.clicksNum++;
        if (stats.maxCombo < stats.combo) stats.maxCombo = stats.combo;
        stats.combo = 0;
    }

    public void Skip() {
        stats.skippedNotes++;
        if (stats.maxCombo < stats.combo) stats.maxCombo = stats.combo;
        stats.combo = 0;
    }

    private void SetPulsorColorid(int id) {
        pulsor.color = noteColors[id];
    }

    IEnumerator ShowPoints(float scores, float posX, float posY) {
        GameObject scoresgo = Instantiate(scoresShowerPrototype, new Vector3(0, 0, 0), Quaternion.identity, scoresShowerPrototype.transform.parent);
        scoresgo.SetActive(true);
        scoresgo.transform.position = new Vector2(posX, posY);

        scoresgo.transform.GetComponent<Text>().text = string.Format("{0:0.00}", scores);
        RectTransform scoresgopos = scoresgo.transform.GetComponent<RectTransform>();
        Text text = scoresgopos.transform.GetComponent<Text>();

        float t = 0;
        float localPosY = heightOfScoresAppearing;
        while(t < 1.0) {
            t += Time.deltaTime / secondsWhenScoresAreShown;
            scoresgopos.localPosition = new Vector2(scoresgopos.localPosition.x, heightOfScoresAppearing + t * howFarScoresFlyOut);
            text.color = new Color (1, 1, 1, 1f - t);
            yield return null;
        }
        Destroy(scoresgo);
    }

    IEnumerator SetUpPulsor() {

        while (true) {
            pulsor.color = new Color(pulsor.color.r, pulsor.color.g, pulsor.color.b, pulsor.color.a - (Time.deltaTime/coefOfSpeedToDecreasePulsorsAlpha));
            yield return null;
        }

        yield return null;
    }
}