using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    bool buttonPushed=false;
   public GameObject rock;
    Vector3 initialTransformPosition;
    Vector3 finalTransform=new Vector3(0.09928384f, 0.1f,0);
    public Vector2 newRockPos;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        initialTransformPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPushed)
        {
            rock.transform.position = Vector2.Lerp(rock.transform.position, newRockPos, 0.2f);
        }

        if (player == null)
        {
            transform.position = initialTransformPosition;
            buttonPushed = false;
        }

        while (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)

            { break; }

            else if (GameObject.FindGameObjectWithTag("Player") != null)
            { player = GameObject.FindGameObjectWithTag("Player"); }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            buttonPushed = true;
        }
    }
}
