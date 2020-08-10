using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    #region horizontal Movement Decl
    public float horizSpeed=5;
    public float jumpForce=5;
    private float xMovementDirection;
    Rigidbody2D rb2d;
    bool isfacingRight = true;
    #endregion

    #region jump Decl
    bool isGrounded;
    public Transform groundCheck;
    float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;
    int jumpsLeft;
   public bool canMultiJump=false;
    public int maxJumps;
    #endregion

    #region dash Decl
    public float dashSpeed=15f;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        xMovementDirection = Input.GetAxis("Horizontal");

        if(!isDashing)
        rb2d.velocity = new Vector2(xMovementDirection * horizSpeed, rb2d.velocity.y);


        #region flipPlayerSprite
        if (!isfacingRight && xMovementDirection>0)
        {
            Flip();
        }
        else if(isfacingRight && xMovementDirection < 0)
        {
            Flip();
        }
        #endregion
    }


    private void Update()
    {
        #region Jumping
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
        if(Input.GetButtonDown("ArrowJump") && jumpsLeft > 0)
        {
         
            rb2d.velocity = Vector2.up * jumpForce;
            jumpsLeft--;
        }
        #endregion

        #region Dash
        
        if (Input.GetButton("Dash") && remainingDash>0)
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
}
