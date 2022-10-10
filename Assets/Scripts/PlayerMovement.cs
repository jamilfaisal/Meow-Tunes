using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioSource jumpSound1;
    public AudioSource jumpSound2;
    public AudioSource jumpSound3;
    public AudioSource jumpSound4;
    private AudioSource[] jumpSounds;
    private int lastJumpSound = -1;
    
    public AudioSource landFromJumpSound;
    private bool _justLanded = false;

    public AudioSource walkingSound;
    private bool _isMoving;
    
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float maxJumpForce;
    private bool jumpKeyHeld;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool canDoubleJump;
    public float stompForce = 3f;
    public float jumpingGravity;

    [Header("Keybinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode stompKey = KeyCode.Tab;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerWidth;
    public LayerMask whatIsGround;
    private bool grounded;
    private RaycastHit sphereHit;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    //Tracking current player's movement
    public enum MovementState {
        walking,
        sprinting,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        canDoubleJump = false;
        
        jumpSounds = new[] { jumpSound1, jumpSound2, jumpSound3, jumpSound4 };
    }

    private void Update()
    {
        // ground check shoot a sphere to the foot of the player
        // Cast origin and the sphere must not overlap for it to work, thus we make the origin higher
        float sphereCastRadius = playerWidth * 0.5f;
        float sphereCastTravelDist = playerHeight * 0.5f - playerWidth * 0.5f + 0.3f;
        grounded = Physics.SphereCast(transform.position + Vector3.up*sphereCastTravelDist*2, sphereCastRadius, Vector3.down, out sphereHit, sphereCastTravelDist*2.1f);
        
        MyInput();
        SpeedControl();
        MovementStateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (_justLanded && grounded)
        {
            landFromJumpSound.Play();
            _justLanded = false;
        }
        
        if (rb.velocity.magnitude > 1 && grounded && !walkingSound.isPlaying) walkingSound.Play();
        if (rb.velocity.magnitude <= 0 || !grounded) walkingSound.Stop();
    
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if(!grounded && !jumpKeyHeld &&Vector3.Dot(rb.velocity, Vector3.up) > 0){
            rb.AddForce(Vector3.down * 30f * rb.mass);
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetButtonDown("Jump")){
            jumpKeyHeld = true;

            if(readyToJump && grounded){

                readyToJump = false;
                canDoubleJump = true;

                Jump();
                pickJumpSound().Play();

                Invoke(nameof(ResetJump), jumpCooldown);

            }

            if(canDoubleJump && !grounded){

                canDoubleJump = false;

                DoubleJump();
                pickJumpSound().Play();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
        else if(Input.GetButtonUp("Jump")){
            jumpKeyHeld = false;
        } else if (Input.GetKey(stompKey))
        {
            Stomp();
        }
    }

    private AudioSource pickJumpSound() {
        int jumpSoundIndex = -1;
        if (lastJumpSound == -1) {
            jumpSoundIndex = Random.Range(0, 3);
        } else {
            jumpSoundIndex = Random.Range(0, 3);
            if (jumpSoundIndex == lastJumpSound) {
                jumpSoundIndex = (jumpSoundIndex + Random.Range(1, 3)) % 4;
            }
        }
        lastJumpSound = jumpSoundIndex;
        return jumpSounds[jumpSoundIndex];
    }

    private void MovementStateHandler(){
        
        //Sprinting
        if(grounded && Input.GetKey(sprintKey)){
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        //Walking
        else if(grounded){
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        //in air
        else{
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope and not jumping
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            //Prevents weird jumping motion due to no gravity on slope
            if(rb.velocity.y > 0){
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // on ground
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //Add some additional gravity to not make the control floaty
            rb.AddForce(Vector3.down * jumpingGravity * rb.mass);


        //Turn off Gravity when on slope to avoid unwanted sliding
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limit velocity on slope (except jumping to prevent limiting the jump)
        if(OnSlope() && !exitingSlope){
            if(rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limit velocity on ground or air
        else{
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Stomp()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.up * stompForce, ForceMode.Impulse);
    }

    private void Jump()
    {
        exitingSlope = true;
        
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * maxJumpForce, ForceMode.Impulse);

    }
    private void DoubleJump()
    {
        exitingSlope = true;
        
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * (maxJumpForce/2), ForceMode.Impulse);

    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
        _justLanded = true;
    }

    private bool OnSlope()
    {   
        //Possible Extension: Check going up or down slope to change speed base on that

        //Increase the length of the ray a bit to make sure we are hitting the slope 
        // (may need to change based on ramp slope)
        float RayExtension = 0.3f;

        // slopeHit stores the information of the object the ray hits
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + RayExtension))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        //Project the move direction to the slope so that we are not moving into or away from the slope
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}