using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpyPlayerMovementTest2 : MonoBehaviour
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
    bool currentlyHangingOffLedge = false;
    bool ledgePositionSecured = false;

    public Transform groundCheck;
    public Transform ledgeCheck;
    public Transform wallCheck;



    float groundCheckRadius = 0.5f;
    float wallCheckDistance = 0.8f;
    float ledgeCheckDistance = 0.8f;
    public LayerMask groundLayer;


    Animator anim;

    bool canMove = true;
    bool canFlip = true;

    int jumpsLeft;
    public int maxJumps;
    public float jumpForce = 5;


    bool isHanging;

    float xPosTranslation=1f;
    Vector3 holdingPos;
    #endregion



    #region dash Decl

    public float dashSpeed = 40f;
    public float maxdashTime = 0.8f;
    float remainingDash;
    bool isDashing;

    public float distanceBetweenImages;
    private float lastImageXpos;
    private float dashTimeLeft;
    private bool canDash = true;
    private int facingDirection = 1;
    private int amountOfDashesLeft;
    public int amountOfDashes = 1;
    private float lastDash = -100f;
    public float dashTime=0.5f;
    public float dashCoolDown;
    #endregion


    bool isHovering;
    public float hoveringSpeed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
        remainingDash = maxdashTime;
        anim = GetComponent<Animator>();
        amountOfDashesLeft = amountOfDashes;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isWall = Physics2D.Raycast(wallCheck.position, new Vector2(Input.GetAxisRaw("Horizontal"),0), wallCheckDistance, groundLayer);
        isNoLedge = Physics2D.Raycast(ledgeCheck.position,new Vector2(Input.GetAxisRaw("Horizontal"),0), ledgeCheckDistance, groundLayer);
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
        CheckIfCanDash();
        CheckDash();

        #region Jump and Hover
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
       

        if (Input.GetButton("ArrowJump")|| Input.GetButton("Jump"))
       
        {
            if (rb2d.velocity.y < -hoveringSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -hoveringSpeed);
            }
        }
        #endregion

        #region Ledge Hang
        if (isWall && !isNoLedge && !isHanging)
        {
           
            isHanging = true;
            canMove = false;
            canFlip = false;
            holdingPos = transform.position;
        }

        if (isHanging == true )
        {
           
            transform.position = holdingPos;

            if (Input.GetButtonDown("ArrowJump") || Input.GetButtonDown("Jump"))
            {
                isHanging = false;
                if (isfacingRight)
                {
                    transform.position = new Vector3(transform.position.x + xPosTranslation, ledgeCheck.position.y);
                }
                if (!isfacingRight)
                {
                    transform.position = new Vector3(transform.position.x - xPosTranslation, ledgeCheck.position.y);
                }
                canMove = true;
                canFlip = true;
            }
        }


        #endregion
        #region Dash
        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown))
                AttemptToDash();
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
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * Input.GetAxisRaw("Horizontal"), wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + ledgeCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            Destroy(collision.gameObject);
        }
    }

    private void CheckDash()
    {
        if (isDashing && canDash)
        {
            if (dashTimeLeft > 0)
            {


                canMove = false;
                canFlip = false;
                rb2d.velocity = new Vector2(dashSpeed * Input.GetAxis("Horizontal"), 0.0f);
                
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isWall)
            {
                amountOfDashesLeft--;
                isDashing = false;
                canMove = true;
                canFlip = true;

                rb2d.velocity = Vector2.zero;
            }
        }
        else
        {
            isDashing = false;
        }

    }
        private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    private void CheckIfCanDash()
    {
        if (isGrounded)
        {
            amountOfDashesLeft = amountOfDashes;
        }

        if (amountOfDashesLeft <= 0)
        {
            canDash = false;
        }
        else
        {
            canDash = true;
        }
    }

    public int GetFacingDirection()
    {
    
        return facingDirection;
    }

}



/* if (Input.GetButton("Dash") && remainingDash > 0)
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

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;

            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        }*/
