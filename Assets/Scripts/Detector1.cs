using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is to detect gameobjects like notes, frozes
public class Detector1 : MonoBehaviour
{
    [SerializeField] FrozeButton frozeButton;
    [SerializeField] Concluder concluder;
    [SerializeField] GameProcess gameProcess;

    [System.NonSerialized]
    public List<GameObject> NotesListInsideTheTrigger;
    public float detectionThickness;

    private void Start() {
        NotesListInsideTheTrigger = new List<GameObject>();
    }

    // Detects what was triggered
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "N") {
            NotesListInsideTheTrigger.Add(collider.gameObject);
        } else if (collider.gameObject.name == "F") {
            frozeButton.FrozeButtonWithIndex(int.Parse(collider.transform.GetChild(0).name));
        } else if (collider.gameObject.name == "END") {
            concluder.GetResults();
        }
    }

    // Checks what note has been pressed
    public void CheckTrigger(int id) {
        bool isFound = false;

        foreach (GameObject go in NotesListInsideTheTrigger)
        {
            if (go.transform.GetChild(0).name == id.ToString()) {
                isFound = true;
                gameProcess.Hit(this.transform.position.x, go.transform.position.x, detectionThickness*50, id);

                NotesListInsideTheTrigger.Remove(go);
                Destroy(go);
                break;
            }
        }

        if (!isFound) {
            gameProcess.Miss();
        }
    }

    // Set the thickness of detector
    public void SetDetectorThickness(float thickness) {
        detectionThickness = thickness;
        this.transform.parent.gameObject.transform.localScale = new Vector3(thickness, 1, 1);
    }
}