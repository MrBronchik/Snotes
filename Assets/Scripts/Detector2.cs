using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector2 : MonoBehaviour
{
    [SerializeField] FrozeButton frozeButton;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "F") {
            frozeButton.FrozeButtonWithIndex(int.Parse(collider.transform.GetChild(0).name));
        }
    }
}
