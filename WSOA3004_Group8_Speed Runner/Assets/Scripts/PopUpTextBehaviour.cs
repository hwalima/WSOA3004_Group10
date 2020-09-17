using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextBehaviour : MonoBehaviour
{
   Transform popUpTextPosition;
    // Start is called before the first frame update
    void Start()
    {
        popUpTextPosition = GameObject.FindGameObjectWithTag("PopUpTextPos").transform; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 thisObjectPos = Camera.main.WorldToScreenPoint(popUpTextPosition.position);
        transform.position = thisObjectPos;
    }
}
