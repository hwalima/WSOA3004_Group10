using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtHarpy : MonoBehaviour
{

    public AIOne aiOneScript;
    GameObject player;
    float damping = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (aiOneScript.isChasingPlayer == true)
        {
            if (player != null)
            {

                //  transform.LookAt(new Vector3( player.transform.position.x,0,0));
                Vector2 direction = new Vector2(player.transform.position.x - transform.position.x+180, player.transform.position.y - transform.position.y+180);
                transform.up = direction;
            }
        }
        else if (aiOneScript.isChasingPlayer == false)
        {
            transform.rotation = Quaternion.identity;
        }


        while (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)

            { break; }

            else if (GameObject.FindGameObjectWithTag("Player") != null)
            { player = GameObject.FindGameObjectWithTag("Player"); }

        }
    }
}
