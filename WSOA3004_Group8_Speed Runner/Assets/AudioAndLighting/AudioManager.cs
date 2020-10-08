using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public static AudioManager audInstance;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {

        DontDestroyOnLoad(gameObject);
        if (audInstance == null)
        {
            audInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound ssound in sounds)
        {
            ssound.audSource = gameObject.AddComponent<AudioSource>();


            ssound.audSource.clip = ssound.audclip;
            ssound.audSource.volume = ssound.volume;
            ssound.audSource.pitch = ssound.pitch;
            ssound.audSource.loop = ssound.loop;

        }
    }

    private void Start()
    {
        MakeSound("Main music");
     //   MakeSound("Ruins music");
    }



    public void MakeSound(string name)
    {
        Sound ssoud = Array.Find(sounds, sound => sound.name == name);
        if (ssoud == null)
        {
            return;
        }
        ssoud.audSource.Play();

    }

    public void StopSound(string name)
    {
        Sound ssoud = Array.Find(sounds, sound => sound.name == name);
        
        ssoud.audSource.Stop();
    }

  /*  public void GoToGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    */

   
}
