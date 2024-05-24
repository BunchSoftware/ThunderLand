using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup Mixer;

    public void Pause()
   {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Mixer.audioMixer.SetFloat("Music", -80f);
        }
        else
        {
            Mixer.audioMixer.SetFloat("Music", 0f);
            Time.timeScale = 1;
        }
    }
   public void LoadScene(int scene)
   {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
}
