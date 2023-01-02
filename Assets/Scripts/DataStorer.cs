using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for storing data like lvl's logo, that can't be passed by player prefs
public class DataStorer : MonoBehaviour
{
    public static DataStorer instance;

    private void Awake() {
        if (instance != null) {
            Destroy(instance);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
}
