using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : MonoBehaviour
{

    Vector3 initialPosition;
    public float speed = 2f;

    public Transform nearestCheckPoint;

    Transform player;

    public bool moveLeft;//move the rock to the left if it passes a certain point
    public bool moveRight;//move the rock right if it moves past a certain point

    float moveDownPos = -125f;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (moveLeft)
        {
            if (transform.position.x < initialPosition.x)
            {
                transform.position = Vector3.Lerp(transform.position, initialPosition, speed * Time.deltaTime);
            }
        }

        if (moveRight)
        {
            if (transform.position.x > initialPosition.x)
            {
                transform.position = Vector3.Lerp(transform.position, initialPosition, speed * Time.deltaTime);
            }
        }

        if (transform.position != initialPosition)
        {
            if (player == null)
            {
                transform.position = initialPosition;
            }
        }
       /* if (player == null)
        {
            StartCoroutine(WaitForPlayerToRespawn());
        }
        */
        while (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)

            { break; }

            else if (GameObject.FindGameObjectWithTag("Player") != null)
            { player = GameObject.FindGameObjectWithTag("Player").transform; }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            transform.position = new Vector2(initialPosition.x, moveDownPos);
        }
    }

    /*IEnumerator WaitForPlayerToRespawn()
    {
        yield return new WaitForSeconds(2f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }*/
}
