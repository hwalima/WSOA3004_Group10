using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaliHeadControl : MonoBehaviour
{
    public Transform fovEmpty;
   GameObject player;
    Vector3 playersInitialPosition;
    bool playerPositionRecorded = false;

    public float speed = 20;
    public bool isAttacking;
    public bool waitBeforeAttacking;
    public bool finishedAttacking;


    bool dad = false;

    Vector3 predictedPosition;
    public GeneralDaliManager generalDaliManager;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.tag == "DadDali")
        {
            dad = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] detectPlayer = Physics2D.OverlapAreaAll(transform.position, fovEmpty.transform.position);

        if (detectPlayer.Length > 0)
        {
            for (int i = 0; i < detectPlayer.Length; i++)
            {
                if (detectPlayer[i].transform.gameObject.tag == "Player")
                {
                    this.transform.parent = null;
                    player = detectPlayer[i].gameObject;
                    //chase after the player
                    if (!waitBeforeAttacking)
                    {
                        if (!dad)
                        {
                            isAttacking = true;
                            ChasePlayer();
                        }

                        else
                        {
                            isAttacking = true;
                            DadChasePlayer();
                        }
                    }


                }
            }
        }
        if (playerPositionRecorded)
        {
            if (!dad)
            {
                transform.position = Vector3.MoveTowards(transform.position, playersInitialPosition, speed * Time.deltaTime);
                if (transform.position == playersInitialPosition)
                {
                    StartCoroutine(WaitBeforeDestroy());
                }
            }
            if (dad)
            {
                transform.position = Vector3.MoveTowards(transform.position, predictedPosition, speed * Time.deltaTime);
                if (transform.position == playersInitialPosition)
                {
                    StartCoroutine(WaitBeforeDestroy());
                }
            }
        }
    }

    void ChasePlayer()
    {
        if (!playerPositionRecorded)
        {
            playersInitialPosition = player.transform.position;
            playerPositionRecorded = true;
        }
    }

    void DadChasePlayer()
    {
        if (!playerPositionRecorded)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float estimatedTime = distance / speed;
            Rigidbody2D playerrb = player.GetComponent<Rigidbody2D>();
            Debug.Log(playerrb);
            predictedPosition = new Vector2(player.transform.position.x, player.transform.position.y) + (playerrb.velocity * estimatedTime);
            playerPositionRecorded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject == player)
            {

                StartCoroutine(WaitBeforeDestroy());
            }
        
    }
    private void OnDrawGizmos()
    {

    }

    IEnumerator WaitBeforeDestroy()
    {
        generalDaliManager.next = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!dad)
        { ChasePlayer();
            this.transform.parent = null;
        }

        if (dad)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            DadChasePlayer();
            this.transform.parent = null;
        }
    }
}
