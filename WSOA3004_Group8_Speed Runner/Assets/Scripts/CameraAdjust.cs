using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAdjust : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public float camSize;
    public float prevCamSize;
    public float speed;
    public bool lerpCam = false;
    private bool prevCam = false;
    private float t = 0f;
    

    // Update is called once per frame
    void Update()
    {
        
        if (lerpCam == true && prevCam == false) //if the player goes forward, it'll swap to the camera size for the next area
        {
            t += speed * Time.deltaTime;
            cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, camSize, speed * Time.deltaTime);
            if (t >= 1)
            {
                lerpCam = false;
                t = 0;
            }
        }
        else if (lerpCam == true && prevCam == true) //if the player goes backwards, it'll swap back to the camera size of the previous area
        {
            t += speed * Time.deltaTime;
            cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, prevCamSize, speed * Time.deltaTime);
            if (t >= 1)
            {
                lerpCam = false;
                t = 0;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<Rigidbody2D>().velocity.x > 0) //check if player moving right
            {
                lerpCam = true;
                prevCam = false;
                //cam.m_Lens.OrthographicSize = camSize;
            }
            else if (collision.GetComponent<Rigidbody2D>().velocity.x < 0) //check if player moving left
            {
                lerpCam = true;
                prevCam = true;
                //cam.m_Lens.OrthographicSize = prevCamSize;
                
            }
            
        }
        
    }

    
   
}
