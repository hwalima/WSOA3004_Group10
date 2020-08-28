﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private float movementInputDirection;
    private float vertInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    private int amountOfJumpsLeft;
    private int amountOfDashesLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isHovering;
    private bool canHover;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool isDashing;
    private bool canDash = true;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;
    public int amountOfDashes = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float hoveringSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;


    private GameManager GM;
    [SerializeField]
    private GameObject respawnPoint;

    //stamina 

    [SerializeField] private float stamina = 100f;
    private float currentStamina;
    [SerializeField] private Slider StaminaSlider;
    [SerializeField] private float staminaUseRate;
    [SerializeField] private float staminaRefillRate;
    private bool canRefillStamina = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        respawnPoint = GameObject.Find("RespawnPoint");
        amountOfJumpsLeft = amountOfJumps;
        amountOfDashesLeft = amountOfDashes;
        currentStamina = stamina;
        wallHopDirection.Normalize();
        StaminaSlider = GameObject.Find("Canvas/StaminaSlider").GetComponent<Slider>();
        wallJumpDirection.Normalize();
        transform.parent = this.transform;
    }

    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckJump();
        CheckIfWallSliding();
        CheckStamina();
        CheckRefillStamina();
        RefillStamina();
        CheckIfHovering();
        CheckIfCanDash();
        CheckDash();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckStamina()
    {
        StaminaSlider.value = currentStamina / stamina;
    }

    private void CheckIfHovering()
    {
        if (canHover && Input.GetButton("Jump") && rb.velocity.y < 0 && currentStamina > 0f)
        {
            isHovering = true;
        }
        else
        {
            isHovering = false;
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && /*movementInputDirection == facingDirection &&*/ rb.velocity.y < 0 && vertInputDirection != -1) //allows for player to instantly stick to wall when they jump uncomment to make it that you need to move towards the wall to slide 
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isHovering", isHovering);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        vertInputDirection = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall) // was getbuttondown
        {
          
            if (!isGrounded && movementInputDirection != facingDirection )
            {
                
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0) //there was no !istouching wall
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown))
                AttemptToDash();
        }

    }

    private void CheckRefillStamina()
    {
        if (isGrounded)
        {
            canRefillStamina = true;
        }
        else if (isHovering)
        {
            canRefillStamina = false;
        }
    }

    private void RefillStamina()
    {
        if (canRefillStamina)
        {
            if (currentStamina < stamina)
            {
                currentStamina += staminaRefillRate * Time.deltaTime;
            }
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DashReset"))
        {
            amountOfDashesLeft++;
            Destroy(other.gameObject);
            Debug.Log("DashyDash");
        }

        if(other.gameObject.CompareTag("Death"))
        {
            Die();
        }

        if(other.gameObject.CompareTag("CheckPoint"))
        {
            Debug.Log("CheckPoint");
            respawnPoint.transform.position = other.gameObject.transform.position;
        }
    }

    private void Die()
    {
        //Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);   //can be used for effects on death
        //Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.Respawn();
        Destroy(gameObject);
    }

    private void AttemptToDash()
    {
        if (canDash)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {


                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                amountOfDashesLeft--;
                isDashing = false;
                canMove = true;
                canFlip = true;

                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            isDashing = false;
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            //WallJump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if(!isGrounded && isTouchingWall && (movementInputDirection == facingDirection || movementInputDirection == 0))
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            canHover = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            //   Debug.Log("has JUmped");
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            canHover = true;
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true; 
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
            Flip();
            //  Debug.Log("has WallJUmped");
        }
    }

    private void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }


        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
                canHover = false;
            }
        }

        if (isHovering)
        {
            if (rb.velocity.y < -hoveringSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -hoveringSpeed);
                currentStamina -= staminaUseRate * Time.deltaTime;
            }
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(wallCheck && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector3.zero;
        }
    }

}
