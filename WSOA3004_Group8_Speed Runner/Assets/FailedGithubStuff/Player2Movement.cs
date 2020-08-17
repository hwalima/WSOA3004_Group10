using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    #region horizontal Movement Decl
    public float horizSpeed = 5;
    public float jumpForce = 5;
    private float xMovementDirection;
    Rigidbody2D rb2d;
    bool isfacingRight = true;
    #endregion

    #region jump Decl
    bool isGrounded;
    public Transform groundCheck;
    float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;
    public float maxJumpTime = 0.2f;
    float currentJumpTime;
   
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentJumpTime = maxJumpTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        xMovementDirection = Input.GetAxis("Horizontal");

       
            rb2d.velocity = new Vector2(xMovementDirection * horizSpeed, rb2d.velocity.y);

        #region flipPlayerSprite
        if (!isfacingRight && xMovementDirection > 0)
        {
            Flip();
        }
        else if (isfacingRight && xMovementDirection < 0)
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
            currentJumpTime = maxJumpTime;
        }
        
        if (Input.GetButton("ArrowJump") && currentJumpTime>0)
        {
            rb2d.velocity = Vector2.up * jumpForce;
            currentJumpTime -= Time.deltaTime;
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
