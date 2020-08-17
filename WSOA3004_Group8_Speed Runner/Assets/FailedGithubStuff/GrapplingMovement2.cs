using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingMovement2 : MonoBehaviour
{
    public float grappleMaxDistance;
    public LayerMask grappleAbleLayer;

    RaycastHit2D hit2D;
    bool horizontalGrapple=false;
    float timeTakenToReelIn = 1f;

    bool isLeft = false;
    bool verifyIsleft = false;
    public LineRenderer lineRendererside;


    #region UpDOwnMovement
    RaycastHit2D verticalhit2D;
    bool verticalGrapple = false;
   

    bool isUp = false;
    bool verifyIsUp = false;
    public LineRenderer lineRendererUp;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        #region moveLeftAndRight
        if (Input.GetButtonDown("Horizontal"))
        {
            if (!horizontalGrapple)
            {
                ThrowHorizontalGrapplingHook();
                return;
            }
            else if (horizontalGrapple)
            {
                ReelInHoriz();
            }
        }

        if (Input.GetButtonDown("Horizontal") && horizontalGrapple==true)
        {
            ReelInHoriz();
        }
        #endregion


        #region moveUpAndDown
        if (Input.GetButtonDown("Vertical"))
        {
            if (!verticalGrapple)
            {
                ThrowVerticalGrapplingHook();
                return;
            }
            else if (verticalGrapple)
            {
                ReelInVert();
            }
        }

        if (Input.GetButtonDown("Vertical") && verticalGrapple == true)
        {
            ReelInVert();
        }
        #endregion

    }

    #region Horizontal Grappling
    void ThrowHorizontalGrapplingHook()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), 0);
        hit2D = Physics2D.Raycast(transform.position,
             dir * grappleMaxDistance, grappleMaxDistance, grappleAbleLayer);

        if (dir.x > 0)
        {
            isLeft = false;
        }
        else if (dir.x < 0)
        {
            isLeft = true;
        }

        if (hit2D)
        {
            horizontalGrapple = true;
            if(hit2D)
                lineRendererside.positionCount=2;
            lineRendererside.SetPosition(0, transform.position);
            lineRendererside.SetPosition(1, hit2D.point);
            Debug.Log(hit2D.point);
        }
    }

    void ReelInHoriz()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (dir.x > 0)
        {
           verifyIsleft = false;
        }
        else if (dir.x < 0)
        {
            verifyIsleft = true;
        }


        if (isLeft == verifyIsleft)
        {
            //if player hasnt changed the direction they want to go in
            transform.position = Vector3.Lerp(transform.position, hit2D.point, timeTakenToReelIn);
        }
        lineRendererside.positionCount = 0;
        horizontalGrapple = false;

    }
    #endregion


    #region Vertical Grappling

    void ThrowVerticalGrapplingHook()
    {
        Vector2 dir = new Vector2(0, Input.GetAxis("Vertical"));
        verticalhit2D = Physics2D.Raycast(transform.position,
             dir * grappleMaxDistance, grappleMaxDistance, grappleAbleLayer);

        if (dir.y > 0)
        {
            isUp = false;
        }
        else if (dir.y < 0)
        {
            isUp = true;
        }

        if (verticalhit2D)
        {
            verticalGrapple = true;
            lineRendererUp.positionCount = 2;
            lineRendererUp.SetPosition(0, transform.position);
            lineRendererUp.SetPosition(1, verticalhit2D.point);
            Debug.Log(verticalhit2D.point);
        }
    }

    //unfixed
    void ReelInVert()
    {
        Vector2 dir = new Vector2(0, Input.GetAxis("Vertical"));
        if (dir.y > 0)
        {
            verifyIsUp = false;
        }
        else if (dir.y < 0)
        {
            verifyIsUp = true;
        }


        if (isUp == verifyIsUp)
        {
            //if player hasnt changed the direction they want to go in
            transform.position = Vector3.Lerp(transform.position, new Vector2(verticalhit2D.point.x, verticalhit2D.point.y+0.7f), timeTakenToReelIn);
        }
       lineRendererUp.positionCount = 0;
        verticalGrapple = false;
           

    }
    #endregion
}
