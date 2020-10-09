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

    private void Start()
    {
       
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        unlockedAbilityTextHolder.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckRespawn();
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
            var playerTemp = Instantiate(player, respawnPoint.transform.position, respawnPoint.transform.rotation);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }

    public void CollectedStick(int sticksCollected)
    {
        collectedSticks = collectedSticks + sticksCollected;
        sticksCollectedText.text = collectedSticks.ToString();
        //playableDirector.Play();
        StartCoroutine(NewAbilityText());

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
        unlockedAbilityTextHolder.text = UnlockedAbilityWhatToSay[collectedSticks - 1];

        yield return new WaitForSeconds(1f);
        unlockedAbilityTextHolder.gameObject.SetActive(false);
    }
}
