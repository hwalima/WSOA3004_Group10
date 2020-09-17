using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : MonoBehaviour
{

    Vector3 initialPosition;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      if(transform.position.x<initialPosition.x)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, speed*Time.deltaTime);


        } 
    }
}
