using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class TitleControl : MonoBehaviour {

    public AudioMixer mainMixer;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!)");
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (isFullScreen)
            Debug.Log("Now FullScreen");
        else
            Debug.Log("Not FullScreen");
    }
}
