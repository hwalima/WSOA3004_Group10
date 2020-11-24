using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject meetTheTeamPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("NoelForestLevel");
        GameManager.hopToRuins = false;
    }
    public void SkipToRuins()
    {
        GameManager.hopToRuins = true;
        SceneManager.LoadScene("NoelForestLevel");
    }
    public void MeetTheTeam()
    {
        meetTheTeamPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        meetTheTeamPanel.SetActive(false);
    }
}
