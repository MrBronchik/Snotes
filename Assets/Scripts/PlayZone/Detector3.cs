using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector3 : MonoBehaviour
{
    [SerializeField] Detector1 detector1;
    [SerializeField] GameProcess gameProcess;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name != "N") return;
        detector1.NotesListInsideTheTrigger.Remove(collider.gameObject);
        gameProcess.Skip();
    }
}