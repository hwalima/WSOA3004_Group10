using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    //Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.tag == "Player")
       // {
            Debug.Log("spikey");
            if (this.GetComponent<Animator>())
            {
                Debug.Log("spikey");
                this.GetComponent<Animator>().SetTrigger("collide");
                //this.GetComponent<Animator>().SetTrigger("idle");
            }
       // }
    }
}
