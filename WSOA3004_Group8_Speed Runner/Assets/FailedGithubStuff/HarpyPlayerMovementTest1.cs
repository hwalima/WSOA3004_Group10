using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpyPlayerMovementTest1 : MonoBehaviour
{
    //
    #region horizontal movement Decl
    private float xMovementDirection;
    public float horizSpeed = 5;
    Rigidbody2D rb2d;
    bool isfacingRight = true;
    #endregion


    #region jump and Ledge Hang Decl
    public bool canMultiJump = false;
    bool isGrounded;
    bool isWall;
    bool isNoLedge;
    bool currentlyHangingOffLedge=false;
    bool ledgePositionSecured=false;

    public Transform groundCheck;
    public Transform ledgeCheck;
    public Transform wallCheck;

    Vector2 ledgePosition;
    Vector2 playerLedgeStartPosition;
    Vector2 playerLedgeEndPosition;

    float groundCheckRadius = 0.5f;
    float wallCheckDistance=1f;
    float ledgeCheckDistance=1f;
    public LayerMask groundLayer;


    int jumpsLeft;
    public int maxJumps;
    public float jumpForce = 5;

    public float ledgeClimbxOffsetInitial;
    public float ledgeClimbyOffsetInitial;

    public float ledgeClimbxOffsetFinal;
    public float ledgeClimbyOffsetFinal;

    bool canFlip=true;
    bool canMove=true;

    Animator anim;

    #endregion



    #region dash Decl

    public float dashSpeed = 15f;
    public float maxdashTime = 0.8f;
    float remainingDash;
    bool isDashing;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
        remainingDash = maxdashTime;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isWall= Physics2D.Raycast(wallCheck.position,transform.right, wallCheckDistance, groundLayer);
        isNoLedge = Physics2D.Raycast(ledgeCheck.position,transform.right, ledgeCheckDistance, groundLayer);
        //if true then it is not a ledge, it is just a wall


        #region Horizontal Movement
        if (canMove)
        {
            xMovementDirection = Input.GetAxis("Horizontal");

            if (!isDashing)
                rb2d.velocity = new Vector2(xMovementDirection * horizSpeed, rb2d.velocity.y);
        }
        #endregion

        #region flipPlayerSprite
        if (canFlip)
        {
            if (!isfacingRight && xMovementDirection > 0)
            {
                Flip();
            }
            else if (isfacingRight && xMovementDirection < 0)
            {
                Flip();
            }
        }
        #endregion
    }



    private void Update()
    {
        #region Jumping
        if (canMove)
        {
            if (isGrounded)
            {
                jumpsLeft = maxJumps;
            }

            if (!canMultiJump)
            {
                if (!isGrounded)
                {
                    jumpsLeft = 0;
                    // to stop double jump
                }
            }
            if (Input.GetButtonDown("ArrowJump") && jumpsLeft > 0 || Input.GetButtonDown("Jump") && jumpsLeft > 0)
            {

                rb2d.velocity = Vector2.up * jumpForce;
                jumpsLeft--;
            }
        }
        #endregion


      
        #region Ledge Hang
        if(isWall && !isNoLedge && !currentlyHangingOffLedge)
        {
            currentlyHangingOffLedge = true;
            ledgePosition = wallCheck.position;
            
        }

        if (currentlyHangingOffLedge == true && !ledgePositionSecured)
        {
          
            ledgePositionSecured = true;
            if (isfacingRight)
            {
                playerLedgeStartPosition = new Vector2(Mathf.Floor(ledgePosition.x + wallCheckDistance) - ledgeClimbxOffsetInitial,
                    Mathf.Floor(ledgePosition.y) + ledgeClimbyOffsetInitial);

                playerLedgeEndPosition = new Vector2(Mathf.Floor(ledgePosition.x + wallCheckDistance) + ledgeClimbxOffsetFinal,
                    Mathf.Floor(ledgePosition.y) + ledgeClimbyOffsetFinal);
            }
            else
            {
                playerLedgeStartPosition = new Vector2(Mathf.Ceil(ledgePosition.x - wallCheckDistance) + ledgeClimbxOffsetInitial,
                    Mathf.Floor(ledgePosition.y) + ledgeClimbyOffsetInitial);

                playerLedgeEndPosition = new Vector2(Mathf.Ceil(ledgePosition.x - wallCheckDistance) - ledgeClimbxOffsetFinal,
                  Mathf.Floor(ledgePosition.y) + ledgeClimbyOffsetFinal);
            }
            canMove = false;
            canFlip = false;
            anim.SetBool("ledgePositionSecured", ledgePositionSecured);
        }
        if (ledgePositionSecured)
        {
            transform.position = playerLedgeStartPosition;
            
        }
        #endregion
        #region Dash

        if (Input.GetButton("Dash") && remainingDash > 0)
        {

            isDashing = true;
            if (isfacingRight)
            {
                rb2d.velocity = Vector2.right * dashSpeed;
            }
            else if (!isfacingRight)
            {
                rb2d.velocity = Vector2.left * dashSpeed;
            }
        }

        if (Input.GetButtonUp("Dash"))
        {
            remainingDash = maxdashTime;
            isDashing = false;
        }

        #endregion
    }
    void Flip()
    {
        isfacingRight = !isfacingRight;
        Vector3 scaleValue = transform.localScale;
        scaleValue.x *= -1;
        transform.localScale = scaleValue;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + ledgeCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
    public void FinishLedgelClimb()
    {
        Debug.Log("hhahahahahha");
        ledgePositionSecured = false;
        transform.position = playerLedgeEndPosition;
        canMove = true;
        canFlip = true;
        currentlyHangingOffLedge = false;
        anim.SetBool("ledgePositionSecured", ledgePositionSecured);
    }


}


 

