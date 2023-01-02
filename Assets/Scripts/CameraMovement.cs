using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for camera flow, when level is started
public class CameraMovement : MonoBehaviour
{
    public PlayZoneHandler playZoneHandler;

    public void Play() {
        StartCoroutine(GameObjectFlow());
    }

    IEnumerator GameObjectFlow() {
        while(true) {
            transform.position = new Vector3(transform.position.x + playZoneHandler.speed * Time.deltaTime, 0, 0);

            yield return null;
        }
    }
}
