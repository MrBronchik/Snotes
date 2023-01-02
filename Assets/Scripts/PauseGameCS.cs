using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameCS : MonoBehaviour
{    
    [SerializeField] GameObject pauseUI;
    [SerializeField] AudioSource audioSource;
    
    public bool gamePaused = true;

    // Pause game, show Pause UI
    public void PauseGame() {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseUI.SetActive(true);
        audioSource.Pause();
    }

    // Unpause game, hide Pause UI
    public void UnPauseGame() {
        Time.timeScale = 1f;
        gamePaused = false;
        pauseUI.SetActive(false);
        audioSource.UnPause();
    }
}
