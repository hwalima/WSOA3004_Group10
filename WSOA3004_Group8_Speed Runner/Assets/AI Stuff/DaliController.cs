using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaliController : MonoBehaviour
{
    bool headStillAttached=true;
    float speed = 0.5f;
    public GameObject head;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(headStillAttached)
        transform.Translate(-Vector3.right * speed * Time.deltaTime);

        else
        {

            // legs fall apart animation
            Destroy(gameObject);
        }

        if (!head.transform.IsChildOf(this.transform))
        {
            headStillAttached = false;
        }
    }
}
