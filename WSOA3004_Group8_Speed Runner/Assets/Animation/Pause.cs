using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public GameObject pausedUI;
    private bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        pausedUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = true;
        }

        if (paused)
        {
            pausedUI.SetActive(true);
            Time.timeScale = 0;

        }

      /*  if (!paused)
        {
            pausedUI.SetActive(false);
            Time.timeScale = 1;

        }
        */
    }

    public void Resume()
    {
       // FindObjectOfType<AudioManager>().MakeSound("Select");
        paused = false;
        pausedUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
      //  FindObjectOfType<AudioManager>().MakeSound("Select");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
       // FindObjectOfType<AudioManager>().MakeSound("Select");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
       // FindObjectOfType<AudioManager>().MakeSound("Select");
        Application.Quit();
    }

    public void PauseGame()
    {
      //  FindObjectOfType<AudioManager>().MakeSound("Select");
        paused = true;
    }
}
