using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is to load scenes in game
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private PauseGameCS pauseGameCS;
    public void SwitchToMenu() {
        pauseGameCS.UnPauseGame();
        Loader.Load(Loader.Scene.Menu);
    }

    public void RepeatLevel() {
        Loader.Load(Loader.Scene.PlayZone);
    }
}
