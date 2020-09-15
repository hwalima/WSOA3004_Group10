using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    public float waitTime;
    public bool respawn = true;
    public float waitRespawn;
    private Vector2 startPt;
    // Start is called before the first frame update
    void Awake()
    {
        startPt = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DropWait());
        }
    }

    IEnumerator DropWait()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        yield return new WaitForSeconds(waitTime);
        if (this.GetComponent<Rigidbody2D>())
        {
            //this.GetComponent<Rigidbody2D>().gravityScale = 1;
            this.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            
            if (respawn == true)
            {
                yield return new WaitForSeconds(waitRespawn);
                this.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                transform.position = startPt;
            }
           

        }
    }

   
}
