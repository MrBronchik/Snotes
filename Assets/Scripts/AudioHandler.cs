using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// This is script is for music loading
public class AudioHandler : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;

    public void LoadMusic(string musicPath) {
        StartCoroutine(LoadAudio(musicPath));
    }

    private IEnumerator LoadAudio(string path) {
        WWW request = GetAudioFromFile(path);
        yield return request;

        audioSource.clip = request.GetAudioClip();
    }

    private WWW GetAudioFromFile(string path) {
        string audioToLoad = path;
        WWW request = new WWW(audioToLoad);
        return request;
    }
}