using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadDaliHeadControl : MonoBehaviour
{
    public Transform fovEmpty;
    GameObject player;
    public float speed = 5f;

    bool playerPositionRecorded = false;

    Vector3 predictedPosition;

    public bool isAttacking;
    public bool waitBeforeAttacking;
    public bool finishedAttacking;
    // Start is called before the first frame update
    void Start()
    {
        
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
                    player = detectPlayer[i].gameObject;
                    this.transform.parent = null;
                    //chase after the player
                    if(!waitBeforeAttacking)
                    ChasePlayer();
                }
            }
        }
        if (playerPositionRecorded)
        {
            transform.position = Vector3.MoveTowards(transform.position, predictedPosition, speed * Time.deltaTime);
        }
    }

    void ChasePlayer()
    {
        if (!playerPositionRecorded)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float estimatedTime = distance / speed;
            Rigidbody2D playerrb = player.GetComponent<Rigidbody2D>();
            Debug.Log(playerrb);
            predictedPosition = new Vector2(player.transform.position.x, player.transform.position.y) + (playerrb.velocity * estimatedTime);
            playerPositionRecorded=true;
        }

  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Destroy(gameObject);
        }
    }
}
