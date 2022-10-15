using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraMainTransform;
    [SerializeField]
    private InputActionReference movement;
    [Header("Sound Effects")]
    public AudioSource jumpSound1;
    public AudioSource jumpSound2;
    public AudioSource jumpSound3;
    public AudioSource jumpSound4;
    private AudioSource[] _jumpSounds;
    private int _lastJumpSound = -1;
    
    public AudioSource landFromJumpSound;
    private bool _justLanded;

    public AudioSource walkingSound;
    private bool _isMoving;
    
    [Header("Movement")]
    private float _moveSpeed;
    public float walkSpeed;

    public float groundDrag;

    public float maxJumpForce;
    private bool _jumpKeyHeld;
    public float jumpCooldown;
    public float airMultiplier;
    private bool _readyToJump;
    private bool _canDoubleJump;
    private bool _canSaveJump;
    public float stompForce = 3f;
    public float jumpingGravity;
    public KeyCode stompKey = KeyCode.Tab;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerWidth;
    private bool _grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit _slopeHit;
    private bool _exitingSlope;
    
    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;
    private Rigidbody _rb;

    public MovementState state;

    //Tracking current player's movement
    public enum MovementState {
        Walking,
        Air
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _readyToJump = true;
        _canDoubleJump = false;
        _canSaveJump = true;

        _jumpSounds = new[] { jumpSound1, jumpSound2, jumpSound3, jumpSound4 };
    }


    private void Update()
    {
        // ground check shoot a sphere to the foot of the player
        // Cast origin and the sphere must not overlap for it to work, thus we make the origin higher
        var sphereCastRadius = playerWidth * 0.5f;
        var sphereCastTravelDist = playerHeight * 0.5f - playerWidth * 0.5f + 0.3f;
        _grounded = Physics.SphereCast(transform.position + Vector3.up * (sphereCastTravelDist * 2),
            sphereCastRadius, Vector3.down, out _, sphereCastTravelDist*2.1f);
        
        MyInput();
        SpeedControl();
        MovementStateHandler();

        // handle drag
        if (_grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;

        if (_justLanded && _grounded)
        {
            landFromJumpSound.Play();
            _justLanded = false;
            _canSaveJump = true;
        }
        
        if (_rb.velocity.magnitude > 1 && _grounded && !walkingSound.isPlaying) walkingSound.Play();
        if (_rb.velocity.magnitude <= 0 || !_grounded) walkingSound.Stop();
    
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if(!_grounded && !_jumpKeyHeld &&Vector3.Dot(_rb.velocity, Vector3.up) > 0){
            _rb.AddForce(Vector3.down * (30f * _rb.mass));
        }
    }

    private void MyInput()
    {
        // when to jump
        if(Input.GetButtonDown("Jump")){
            _jumpKeyHeld = true;

            if(_readyToJump && _grounded){

                _readyToJump = false;
                _canDoubleJump = true;
                _canSaveJump = false;

                Jump();
                PickJumpSound().Play();

                Invoke(nameof(ResetJump), jumpCooldown);

            }

	        if((_canDoubleJump || _canSaveJump) && !_grounded){

                _canDoubleJump = false;
                _canSaveJump = false;

                HalfJump();
                PickJumpSound().Play();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

        }
        else if (Input.GetButtonUp("Jump")){
            _jumpKeyHeld = false;
        } else if (Input.GetKey(stompKey))
        {
            Stomp();
        }
    }

    public void TriggerJump(InputAction.CallbackContext context)
    {
        if (!context.started && enabled)
        {
            _jumpKeyHeld = true;

            if(_readyToJump && _grounded){

                _readyToJump = false;
                _canDoubleJump = true;

                Jump();
                PickJumpSound().Play();

                Invoke(nameof(ResetJump), jumpCooldown);
            } else if (context.performed && (_canDoubleJump || _canSaveJump) && !_grounded){

                _canDoubleJump = false;
                _canSaveJump = false;

                HalfJump();
                PickJumpSound().Play();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
            if (context.canceled)
            {
                _jumpKeyHeld = false;
            }
        }
    }

    private AudioSource PickJumpSound() {
        int jumpSoundIndex;
        if (_lastJumpSound == -1) {
            jumpSoundIndex = Random.Range(0, 3);
        } else {
            jumpSoundIndex = Random.Range(0, 3);
            if (jumpSoundIndex == _lastJumpSound) {
                jumpSoundIndex = (jumpSoundIndex + Random.Range(1, 3)) % 4;
            }
        }
        _lastJumpSound = jumpSoundIndex;
        return _jumpSounds[jumpSoundIndex];
    }

    private void MovementStateHandler(){
        
        //Walking
        if (_grounded){
            state = MovementState.Walking;
            _moveSpeed = walkSpeed;
        }
        //in air
        else{
            state = MovementState.Air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction

        var movementControl = movement.action.ReadValue<Vector2>();
        var move = new Vector3(movementControl.x, 0, 0);
        move = cameraMainTransform.right * move.x;
        move.y = 0;

        _moveDirection = move;

        // on slope and not jumping
        if(OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection() * (_moveSpeed * 20f), ForceMode.Force);

            //Prevents weird jumping motion due to no gravity on slope
            if(_rb.velocity.y > 0){
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        switch (_grounded)
        {
            // on ground
            case true:
                _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f), ForceMode.Force);
                break;
            // in air
            case false:
                _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f * airMultiplier), ForceMode.Force);
                break;
        }
        
        //Add some additional gravity to not make the control floaty
        _rb.AddForce(Vector3.down * (jumpingGravity * _rb.mass));
        
        //Turn off Gravity when on slope to avoid unwanted sliding
        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limit velocity on slope (except jumping to prevent limiting the jump)
        if(OnSlope() && !_exitingSlope){
            if(_rb.velocity.magnitude > _moveSpeed)
                _rb.velocity = _rb.velocity.normalized * _moveSpeed;
        }

        // limit velocity on ground or air
        else{
            var velocity = _rb.velocity;
            var flatVel = new Vector3(velocity.x, 0f, velocity.z);

            // limit velocity if needed
            if(flatVel.magnitude > _moveSpeed)
            {
                var limitedVel = flatVel.normalized * _moveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    public void Stomp()
    {
        if (_grounded) return;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(-transform.up * stompForce, ForceMode.Impulse);
    }

    private void Jump()
    {
        _exitingSlope = true;
        
        // reset y velocity
        var velocity = _rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        _rb.velocity = velocity;

        _rb.AddForce(transform.up * maxJumpForce, ForceMode.Impulse);

    }
    private void HalfJump()
    {
        _exitingSlope = true;
        
        // reset y velocity
        var velocity = _rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        _rb.velocity = velocity;

        _rb.AddForce(transform.up * (maxJumpForce/2), ForceMode.Impulse);

    }
    private void ResetJump()
    {
        _readyToJump = true;
        _exitingSlope = false;
        _justLanded = true;
    }

    private bool OnSlope()
    {   
        //Possible Extension: Check going up or down slope to change speed base on that

        //Increase the length of the ray a bit to make sure we are hitting the slope 
        // (may need to change based on ramp slope)
        const float rayExtension = 0.3f;

        // slopeHit stores the information of the object the ray hits
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight * 0.5f + rayExtension))
        {
            var angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        //Project the move direction to the slope so that we are not moving into or away from the slope
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }

}