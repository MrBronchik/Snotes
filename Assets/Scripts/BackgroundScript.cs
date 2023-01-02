using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is only for background (not destroyable)
public class BackgroundScript : MonoBehaviour
{
    public static BackgroundScript instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else if (instance != null) {
            Destroy(this);
        }
    }
}
