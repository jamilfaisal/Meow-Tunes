using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Current;
    
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

    [Header("Movement")]
    public float sidewayWalkSpeed;
    public float forwardWalkSpeed;
    public float[] lanePositions;
    public int currentLane;
    private bool _movingSideway;
    private bool _movePlayerEnabled;

    public float groundDrag;

    public float maxJumpForce;
    public float jumpCooldown;
    // public float airMultiplier;
    private bool _readyToJump;
    //private bool _canDoubleJump;
    private bool _canSaveJump;
    public float stompForce = 3f;
    public float jumpingGravity;
    public KeyCode stompKey;
    [Header("Movement Animation")]
    public Animator animator;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerWidth;
    private bool _grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit _slopeHit;
    // private bool _exitingSlope;
    
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

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        
        //Set lane positions for side movements
        currentLane = 2;
        lanePositions = new float[5];
        lanePositions[0] = GameObject.Find("Lane0").GetComponent<Transform>().position.x;
        lanePositions[1] = GameObject.Find("Lane1").GetComponent<Transform>().position.x;
        lanePositions[2] = GameObject.Find("Lane2").GetComponent<Transform>().position.x;
        lanePositions[3] = GameObject.Find("Lane3").GetComponent<Transform>().position.x;
        lanePositions[4] = GameObject.Find("Lane4").GetComponent<Transform>().position.x;
        _movingSideway = false;
        _movePlayerEnabled = true;

        _readyToJump = true;
        //_canDoubleJump = false;
        _canSaveJump = true;

        animator = GetComponent<Animator>();

        _jumpSounds = new[] { jumpSound1, jumpSound2, jumpSound3, jumpSound4 };
        _rb.drag = groundDrag;
    }


    private void Update()
    {
        // ground check shoot a sphere to the foot of the player
        // Cast origin and the sphere must not overlap for it to work, thus we make the origin higher
        var sphereCastRadius = playerWidth * 0.5f;
        var sphereCastTravelDist = playerHeight * 0.5f - playerWidth * 0.5f + 0.3f;
        _grounded = Physics.SphereCast(transform.position + Vector3.up * (sphereCastTravelDist * 2),
            sphereCastRadius, Vector3.down, out _, sphereCastTravelDist*2.1f);

        if (Time.timeSinceLevelLoad > 5)
        {
            if (GameManager.Current.gameIsEnding)
            {
                return;
            }

            if (_rb.velocity.magnitude > 1 && _grounded && !walkingSound.isPlaying) walkingSound.Play();
            if (_rb.velocity.magnitude <= 0 || !_grounded || GameManager.Current.HasGameEnded()) walkingSound.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > 5 && _movePlayerEnabled){

            if (GameManager.Current.playerIsDying)
            {
                //Add some additional gravity to not make the control floaty
                var velocity = _rb.velocity;
                _rb.velocity = new Vector3(velocity.x, velocity.y, 2f);
                _rb.AddForce(Vector3.down * (20 * _rb.mass));
                return;
            }

            MyInput();

            //Movement
            if (_movingSideway){
                var step =  Mathf.Sqrt(Mathf.Pow(forwardWalkSpeed, 2) + Mathf.Pow(sidewayWalkSpeed, 2)) * Time.fixedDeltaTime;
                Vector3 desiredPosition = _rb.transform.position;
                desiredPosition.x = lanePositions[currentLane];
                var estimatedTime = Mathf.Abs((desiredPosition.x - _rb.transform.position.x)) / sidewayWalkSpeed;
                desiredPosition.z += forwardWalkSpeed * estimatedTime * Time.fixedDeltaTime;
                desiredPosition.y += _rb.velocity.y * estimatedTime;
                _rb.transform.position = Vector3.MoveTowards(_rb.transform.position, desiredPosition, step);

                if (Mathf.Abs(_rb.transform.position.x - desiredPosition.x) < 0.001f){
                    var newPos = _rb.transform.position;
                    newPos.x = desiredPosition.x;
                    _rb.transform.position = newPos;
                    _movingSideway = false;
                    
                    //may be used to solve jump-while-moving-sideway-bug
                    // if (!_readyToJump && _rb.velocity.y > 0){
                    //     _rb.AddForce(transform.up * _rb.velocity.y*2, ForceMode.Impulse);
                    // }
                }
            }
            MovementStateHandler();
            _rb.drag = groundDrag;

            if (_justLanded && _grounded)
            {
                landFromJumpSound.Play();
                _justLanded = false;
            }

            if (_grounded && !_canSaveJump)
            {
                Invoke(nameof(ResetJump), jumpCooldown);
            }

            MovePlayer();
            //Limits downward velocity to be too high (happens sometimes when jumping and switching lanes at the same time)
            if (_rb.velocity.y < -6f){
                var tempVelo = _rb.velocity;
                tempVelo.y = -6f;
                _rb.velocity = tempVelo;
            }
        }
    }

    private void MyInput()
    {
        // when to jump
        if(Input.GetButtonDown("Jump")){
            if(_readyToJump && _grounded){

                _readyToJump = false;
                //_canDoubleJump = true;

                animator.Play("CatJumpFull", 0, 0f);
                Jump();
                PickJumpSound().Play();
                
                Invoke(nameof(SetCanSaveJumpFalse), 0.1f);
            }
            // Commenting out double jump
	        if(_canSaveJump && !_grounded) { // if((_canDoubleJump || _canSaveJump) && !_grounded){

                //_canDoubleJump = false;

                animator.Play("CatJumpFull", 0, 0f);
                HalfJump();
                PickJumpSound().Play();

                Invoke(nameof(SetCanSaveJumpFalse), 0.1f);
            }
        }
        else if (Input.GetKey(stompKey))
        {
            Stomp();
        }
    }

    public void SetCanSaveJumpFalse()
    {
        _canSaveJump = false;
    }

    public void SetMovePlayerEnabledFalse()
    {
        _movePlayerEnabled = false;
    }

    public void SetMovePlayerEnabledTrue()
    {
        _movePlayerEnabled = true;
    }

    public void triggerMove(InputAction.CallbackContext context){
        if (enabled && Time.timeSinceLevelLoad > 5){
            if (context.ReadValue<Vector2>().x < 0 && !_movingSideway && currentLane>0){
                // animator.Play("CatSideJump", 0, 0f);
                _movingSideway = true;
                currentLane -= 1;
            }
            else if (context.ReadValue<Vector2>().x > 0 && !_movingSideway && currentLane<4){
                // animator.Play("CatSideJump", 0, 0f);
                _movingSideway = true;
                currentLane += 1;
            }
        }
    }
    public void TriggerJump(InputAction.CallbackContext context)
    {
        if (!context.started && enabled)
        {
            if(_readyToJump && _grounded){

                _readyToJump = false;
                //_canDoubleJump = true;

                animator.Play("CatJumpFull", 0, 0f);
                Jump();
                PickJumpSound().Play();
                Invoke(nameof(SetCanSaveJumpFalse), 0.1f);
            } 
            // Commenting out double jump
            else if (context.performed && _canSaveJump && !_grounded) { //else if (context.performed && (_canDoubleJump || _canSaveJump) && !_grounded){

                //_canDoubleJump = false;
                _canSaveJump = false;

                animator.Play("CatJumpFull", 0, 0f);
                HalfJump();
                PickJumpSound().Play();
                Invoke(nameof(SetCanSaveJumpFalse), 0.1f);
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

    private void MovementStateHandler()
    {
        //Walking
        state = _grounded ? MovementState.Walking : MovementState.Air;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        // _moveDirection = new Vector3(0, 0, 0);

        // on slope and not jumping
        // if(OnSlope() && !_exitingSlope)
        // {
        //     _rb.AddForce(GetSlopeMoveDirection() * (_moveSpeed * 20f), ForceMode.Force);

        //     //Prevents weird jumping motion due to no gravity on slope
        //     if(_rb.velocity.y > 0){
        //         _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        //     }
        // }
        
        //Add some additional gravity to not make the control floaty
        _rb.AddForce(Vector3.down * (jumpingGravity * _rb.mass));
        
        //Turn off Gravity when on slope to avoid unwanted sliding
        // _rb.useGravity = !OnSlope();

        Vector3 velocity = _rb.velocity;
        velocity.z = forwardWalkSpeed;
        _rb.velocity = velocity;
    }

    public void Stomp()
    {
        if (_grounded) return;
        // reset y velocity
        var velocity = _rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        _rb.velocity = velocity;
        _rb.AddForce(-transform.up * stompForce, ForceMode.Impulse);
    }

    private void Jump()
    {
        // _exitingSlope = true;
        // reset y velocity
        var velocity = new Vector3(0f, 0f, 0f);
        _rb.velocity = velocity;

        _rb.AddForce(transform.up * maxJumpForce, ForceMode.Impulse);
    }
    private void HalfJump()
    {
        // _exitingSlope = true;
        
        // reset y velocity
        var velocity = _rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        _rb.velocity = velocity;

        _rb.AddForce(transform.up * (maxJumpForce/2), ForceMode.Impulse);

    }
    private void ResetJump()
    {
        _readyToJump = true;
        // _exitingSlope = false;
        _justLanded = true;
        _canSaveJump = true;
    }

    // private bool OnSlope()
    // {   
    //     //Possible Extension: Check going up or down slope to change speed base on that

    //     //Increase the length of the ray a bit to make sure we are hitting the slope 
    //     // (may need to change based on ramp slope)
    //     const float rayExtension = 0.3f;

    //     // slopeHit stores the information of the object the ray hits
    //     if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight * 0.5f + rayExtension))
    //     {
    //         var angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
    //         return angle < maxSlopeAngle && angle != 0;
    //     }

    //     return false;
    // }

    // private Vector3 GetSlopeMoveDirection()
    // {
    //     //Project the move direction to the slope so that we are not moving into or away from the slope
    //     return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    // }

}