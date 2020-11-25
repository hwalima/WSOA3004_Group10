using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    public int collectedSticks=0;
    public Text sticksCollectedText;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera CVC;

    public PlayableDirector playableDirector;

   public GameObject popUpText;


    public TextMeshProUGUI unlockedAbilityTextHolder;
    public string[] UnlockedAbilityWhatToSay;

    public string[] AbilityTutorial;
    public TMP_Text tutorialTextComponent;
    public bool isCutscene;
   // int index=0;
    public float typingSpeed=0.01f;
    bool hasWrittenTutorialText=false;


    public TextMeshProUGUI lToContinue;

   public static bool hopToRuins=false;
    Vector2 playerPosHopToRuins =new Vector2(621.47f, 126.16f);

    public GameObject respawnPointParticles;



    private void Start()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        unlockedAbilityTextHolder.gameObject.SetActive(false);

        if (hopToRuins)
        {
            //skip through to go to ruins section
            GameObject.FindGameObjectWithTag("Player").transform.position = playerPosHopToRuins;
            collectedSticks = 5;
            Debug.Log("hop=true");
           // hopToRuins = false;
        }
    }

    private void Update()
    {
        CheckRespawn();
        if (isCutscene)
        {
            if (!hasWrittenTutorialText)
            {
                hasWrittenTutorialText = true;
                StartCoroutine(TypeNewAbility());
              
            }


        }
        if(isCutscene&& Input.GetKeyDown(KeyCode.L))
        {
            isCutscene = false;
            tutorialTextComponent.gameObject.SetActive(false);
            unlockedAbilityTextHolder.gameObject.SetActive(false);
            lToContinue.gameObject.SetActive(false);
            hasWrittenTutorialText = false;
        }

       
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
       
     
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            StartCoroutine(StartParticles());
          //  var playerTemp = Instantiate(player, respawnPoint.transform.position, respawnPoint.transform.rotation);
            // CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }

    public void CollectedStick(int sticksCollected)
    {
        collectedSticks = collectedSticks + sticksCollected;
        sticksCollectedText.text = collectedSticks.ToString();
        //playableDirector.Play();
        StartCoroutine(NewAbilityText());

        isCutscene = true;


    }

    IEnumerator PopUpText()
    {

        popUpText.SetActive(true);
        yield return new WaitForSeconds(2f);
        popUpText.SetActive(false);


    }
    public void SwitchOnPopUpText()
    {
        StartCoroutine(PopUpText());
    }

    IEnumerator NewAbilityText()
    {
        unlockedAbilityTextHolder.gameObject.SetActive(true);
        unlockedAbilityTextHolder.gameObject.transform.position = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Yay i can Text Pos").transform.position);
        unlockedAbilityTextHolder.text = UnlockedAbilityWhatToSay[collectedSticks - 1];
        tutorialTextComponent.gameObject.SetActive(true);
        tutorialTextComponent.gameObject.transform.position= Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("tutorialTextPos").transform.position);
       // tutorialTextComponent.text = AbilityTutorial[collectedSticks - 1];
        lToContinue.gameObject.SetActive(true);
        lToContinue.transform.position = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Player").transform.position)+new Vector3(120,-120f);
        yield return new WaitForSeconds(1f);
      
    }

    public void LastCheckPoint()
    {
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        currentPlayer.transform.position = respawnPoint.transform.position;
    }
    IEnumerator StartParticles()
    {
        respawnPointParticles.SetActive(true);
        respawnPointParticles.transform.position = respawnPoint.transform.position;
        yield return new WaitForSeconds(0.8f);
        var playerTemp = Instantiate(player, respawnPoint.transform.position, respawnPoint.transform.rotation);
        CVC.m_Follow = playerTemp.transform;
        yield return new WaitForSeconds(0.3f);
        respawnPointParticles.SetActive(false);
    }

    IEnumerator TypeNewAbility()
    {

          foreach (char letter in AbilityTutorial[collectedSticks-1].ToCharArray())
            {
            tutorialTextComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            }
    }
}

#region squiggly text incase Ive deleted something serious

/*  tutorialTextComponent.ForceMeshUpdate();
      var textInfo = tutorialTextComponent.textInfo;
      for (int i = 0; i < textInfo.characterCount; ++i)
      {
          var charInfo = textInfo.characterInfo[i];
          if (!charInfo.isVisible)
          {
              continue;
          }
          var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

          for (int j = 0; j < 4; j++)
          {
              var orig = verts[charInfo.vertexIndex + j];
              verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x + 0.01f) * 10f, 0);

          }
      }

      for (int i = 0; i < textInfo.meshInfo.Length; ++i)
      {
          var meshInfo = textInfo.meshInfo[i];
          meshInfo.mesh.vertices = meshInfo.vertices;
          tutorialTextComponent.UpdateGeometry(meshInfo.mesh, i);
      }*/
#endregion