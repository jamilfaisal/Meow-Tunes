using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement current;
    
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
    private float _moveSpeed;
    public float walkSpeed;

    public float groundDrag;

    public float maxJumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool _readyToJump;
    //private bool _canDoubleJump;
    private bool _canSaveJump;
    public float stompForce = 3f;
    public float jumpingGravity;
    public KeyCode stompKey = KeyCode.Tab;
    private int counter = 0;
    private int _lastPosz;

    [Header("Movement")]
    public Animator animator;

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

    public RespawnManager respawnManager;
    public AudioSource respawnSound;
    private AudioClip _respawnClip;
    private bool _respawning = false;
    private int _respawning_cooldown = 5;

    //Tracking current player's movement
    public enum MovementState {
        Walking,
        Air
    }

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _readyToJump = true;
        //_canDoubleJump = false;
        _canSaveJump = true;

        animator = GetComponent<Animator>();

        _jumpSounds = new[] { jumpSound1, jumpSound2, jumpSound3, jumpSound4 };

        _respawnClip = respawnSound.clip;
    }


    private void Update()
    {
        // Debug.Log($"Update:{_rb.velocity}");
        // ground check shoot a sphere to the foot of the player
        // Cast origin and the sphere must not overlap for it to work, thus we make the origin higher
        var sphereCastRadius = playerWidth * 0.5f;
        var sphereCastTravelDist = playerHeight * 0.5f - playerWidth * 0.5f + 0.3f;
        _grounded = Physics.SphereCast(transform.position + Vector3.up * (sphereCastTravelDist * 2),
            sphereCastRadius, Vector3.down, out _, sphereCastTravelDist*2.1f);
        
        if (Time.time > 5){
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
            }

            if (_grounded && !_canSaveJump)
            {
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            
            if (_rb.velocity.magnitude > 1 && _grounded && !walkingSound.isPlaying) walkingSound.Play();
            if (_rb.velocity.magnitude <= 0 || !_grounded || GameManager.current.HasGameEnded()) walkingSound.Stop();

            // _lastPos = transform.position;
        }

    }

    private void FixedUpdate()
    {
        // Debug.Log($"Fixed:{_rb.velocity}");
        if (Time.time > 5){
            if (counter == 0){
                _lastPosz = (int)transform.position.z;
            }
            if (counter == 12){
                if ((int)transform.position.z == _lastPosz)
                {
                    if (!_respawning){
                        Debug.Log("stuck!");
                        LifeManager.current.LostLife();
                        if (LifeManager.current.playerLives != 0)
                        {
                            StartCoroutine(Respawn());
                        }
                        _respawning = true;
                    }
                    else{
                        if(_respawning_cooldown > 0){
                            _respawning_cooldown--;
                        }
                        else{
                            _respawning = false;
                            _respawning_cooldown = 5;
                        }
                    }
                }
            }
            
            MovePlayer();
            var velocity = _rb.velocity;
            if(_grounded){
                _rb.velocity = new Vector3(velocity.x, velocity.y, 8.1F);
            }
            else{
                _rb.velocity = new Vector3(velocity.x, velocity.y, 8);
            }
            if (counter < 12) counter++;
            else counter = 0;
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
        _moveDirection = new Vector3(movementControl.x, 0, 0);

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
                _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 15f), ForceMode.Force);
                break;
            // in air
            case false:
                _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 15f * airMultiplier), ForceMode.Force);
                break;
        }
        
        //Add some additional gravity to not make the control floaty
        _rb.AddForce(Vector3.down * (jumpingGravity * _rb.mass));
        
        //Turn off Gravity when on slope to avoid unwanted sliding
        _rb.useGravity = !OnSlope();

        Vector3 velocity = _rb.velocity;
        velocity.z = 8;
        _rb.velocity = velocity;
    }

    private void SpeedControl()
    {
        // limit velocity on slope (except jumping to prevent limiting the jump)
        if(OnSlope() && !_exitingSlope){
            var velocity = _rb.velocity.normalized * _moveSpeed;
            if(_rb.velocity.magnitude > _moveSpeed)
                _rb.velocity = new Vector3(velocity.x, velocity.y, 8);
        }

        // limit velocity on ground or air
        else{
            var velocity = _rb.velocity;
            var flatVel = new Vector3(velocity.x, 0f, velocity.z);

            // limit velocity if needed
            if(flatVel.magnitude > _moveSpeed)
            {
                var limitedVel = flatVel.normalized * _moveSpeed;
                if (_grounded){
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, 8.1F);
                }
                else{
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, 8);
                }
            }
        }
        // Debug.Log($"Speed:{_rb.velocity}");
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
        _exitingSlope = true;
        // reset y velocity
        var velocity = _rb.velocity;
        velocity = new Vector3(0f, 0f, 0f);
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
        _canSaveJump = true;
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

        // Debug.Log($"Slope:{_rb.velocity}");

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        //Project the move direction to the slope so that we are not moving into or away from the slope
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }

    private IEnumerator Respawn()
    {
        yield return respawnManager.RespawnPlayer(_respawnClip.length);
    }

}