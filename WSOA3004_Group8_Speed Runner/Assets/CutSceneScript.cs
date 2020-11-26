using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneScript : MonoBehaviour
{
    public void StartPlayScene()
    {
        SceneManager.LoadScene("NoelForestLevel");

    }

    public void EndCutScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
