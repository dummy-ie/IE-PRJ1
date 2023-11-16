using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterController2D : MonoBehaviour
{

    [SerializeField] SpriteRenderer _render2D;
    [SerializeField] MeshRenderer _model3D;

    private Rigidbody2D _rb;
    public Rigidbody2D RB { 
        get { return _rb; } 
        set { _rb = value; }
    }

    private CinemachineVirtualCamera _cmVC;
    private Cinemachine3rdPersonFollow _cmTP;

    
    private float _deltaX = 0f;
    private float _deltaY = 0f;

    public float Vertical{ get { return _deltaY; } }

    private float _coyoteTimeCounter = 0f, _jumpBufferCounter = 0f;
    
    private float _vectorShift = 100f;

    private bool _isFacingRight = false;

    private bool _isDashing = false;
    private float _dashTime = 0f;
    private float _dashDuration;
    private bool _aerialDash = true;
    private bool _canDash = true;

    private bool _isJumpPress = false;
    private bool _extraJump = false;
    private bool _isGrounded = false;

    private bool _isHit = false;
    private float _setHitTime = .5f;
    private float _hitTime = .5f;

    private float _iFrames = 0;

    [SerializeField] LayerMask _groundLayer;

    [Header("Stats")]
    [SerializeField] private int _maxHealth = 3;
    public int MaxHealth { get { return _maxHealth; } }
    [SerializeField] private int _currentHealth = 3;
    public int CurrentHealth { get { return _currentHealth; } }
    [SerializeField] private float _maxManite = 100;
    public float MaxManite { get { return _maxManite; } }
    [SerializeField] private float _currentManite = 100;
    public float CurrentManite { get { return _currentManite; } }
    [SerializeField] private bool _hasDash = false;
    public bool HasDash { get { return _hasDash; } }
    [SerializeField] private bool _hasSlash = false;
    public bool HasSlash { get { return _hasSlash; } }

    // have default values for all fields to prevent null errors
    [Header("Movement")]
    [Range(0, 100)][SerializeField] private float _speed = 6f;

    [Header("Jumping")]
    [SerializeField] private bool _allowDoubleJump = false;
    [Range(0, 100)][SerializeField] private float _jumpForce = 14f;
    [Range(0, 10)][SerializeField] private float _fallMultiplier = 4f, _lowJumpMultiplier = 0.8f;
    [Range(0, 5)][SerializeField] private float _coyoteTime = 0.2f, _jumpBufferTime = 0.2f;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float _boxCastDistance = 0.4f;
    [SerializeField] Vector2 _boxSize = new(0.3f, 0.4f);

    [Header("Dashing")]
    [SerializeField] private float _dashDistance = 4f;
    [SerializeField] private float _dashCooldown = .5f;
    [SerializeField] private float _dashCooldownTime = 0f;
    [SerializeField] private float _dashSpeed = 0f;
    [SerializeField] private float _dashOriginalSpeed = 20f;

    public bool IsFacingRight {
        get { return _isFacingRight; }
    }

    private void Awake()
    {
        _cmVC = FindFirstObjectByType<CinemachineVirtualCamera>();
        _cmTP = _cmVC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _rb = GetComponent<Rigidbody2D>();

        _dashSpeed = _dashOriginalSpeed;
        updateDashDuration();
    }

    private void Update()
    {

        Hits();

        if (this._currentHealth <= 0)
        
            this.gameObject.SetActive(false); //OR Destroy(this.gameObject);
        
    }

    private void FixedUpdate() // move player on fixed update so collisions aren't fucky wucky
    {
        Move();
        Jump();
        Dash();
        if (!_isDashing) Flip();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float moveX = context.ReadValue<Vector2>().x;
        float moveY = context.ReadValue<Vector2>().y;
        if (context.started)
        {
            // Debug.Log("Move pressed");
        }
        else if (context.performed)
        {
            // Debug.Log("Moving now" + moveX);

            if (moveX >= 0.5f || moveX <= -0.5f)
                _deltaX = moveX;
            if (moveY >= 0.5f || moveY <= -0.5f)
                _deltaY = moveY;
        }
        else if (context.canceled)
        {
            // Debug.Log("Move released");
            _deltaX = 0;
            _deltaY = 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isJumpPress = true;
            // reset jump buffer counter
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if (context.canceled)
        {
            _isJumpPress = false;
        }
    }
    private void Move()
    {
        if (!(_isDashing || _isHit)) _rb.velocity = new Vector2(_deltaX * _speed, _rb.velocity.y);

        // interpolate the camera towards where the player is currently moving if they are moving?
        // propose the idea later on idk
        //if (horizontal != 0) cmTP.CameraSide = Mathf.Lerp(cmTP.CameraSide, 0.5f * (horizontal + 1f), 0.05f);
    }

    private void Jump()
    {
        // while grounded, set coyote time and jump limit
        if (_isGrounded = IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
            _extraJump = true; // refresh double jump
            _aerialDash = true; //refresh aerialDash
        }
        else
            _coyoteTimeCounter -= Time.deltaTime; // otherwise, count down the coyote time

        if (!IsGrounded())
            _jumpBufferCounter -= Time.deltaTime; // when not pressing jump, count down jump buffering time

        if (!_isJumpPress && _rb.velocity.y > 0f)
        {
            // fall when releasing the jump button early (allows low jumping)
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _lowJumpMultiplier);
            _coyoteTimeCounter = 0f;
        }
        else if (_rb.velocity.y < 0f) // fast fall over time
        {
            _rb.velocity += (_fallMultiplier - 1f) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }

        // Jump if the buffer counter is active AND if coyote time is active OR you have an extra jump AND you can double jump
        if (_jumpBufferCounter > 0f && (_coyoteTimeCounter > 0f || (_extraJump && _allowDoubleJump)))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _jumpBufferCounter = 0f;

            if (!_isGrounded && _coyoteTimeCounter <= 0f)
                _extraJump = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && _canDash && _hasDash) //check if player can dash
        { 
            _dashSpeed = _dashOriginalSpeed;
            updateDashDuration();
            _isDashing = true;
            _aerialDash = false;

            if (!_isFacingRight) //dash direction based on where playr is facing
                _dashSpeed *= -1;
        }
    }
    private void Dash()
    {
        if (_hasDash)
        {
            if (_isDashing)
            {
                _dashTime -= Time.deltaTime;// dash duration countdown

                _rb.velocity = new Vector2(_dashSpeed, 0); // the actual dashing code 

                _dashCooldownTime = _dashCooldown; //set dash cooldown to max dashCooldon

                ShiftTo3D();
            }

            if (_dashTime <= 0) //when player stops dashing
            {

                _dashTime = _dashDuration; //reset dash duration
                _isDashing = false; // player is no longer dashing

                ShiftTo2D();
            }

            if (_dashCooldownTime > 0 && !_isDashing) // ticks down the dash cooldown
            {
                _dashCooldownTime -= Time.deltaTime;
            }

            if (!_isDashing && _dashCooldownTime <= 0 && _aerialDash) _canDash = true; //checks if the player is able to dash
            else _canDash = false;
        }
    }

    void Hits()
    {
        if (_isHit)
        {
            if (_isDashing)
            {
                _isDashing = false;
                ShiftTo2D();
            }
            _hitTime -= Time.deltaTime;
        }

        if (_hitTime <= 0)
        {
            _isHit = false;
            _hitTime = _setHitTime;
        }

        if (_iFrames > 0)
        {
            _iFrames -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        if (_isFacingRight && _deltaX < 0f || !_isFacingRight && _deltaX > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _groundLayer);
    }

    private void updateDashDuration()
    {
       _dashTime = _dashDuration = _dashDistance / _dashSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * _boxCastDistance, _boxSize);
    }

    public void ShiftTo2D()
    {
        _render2D.enabled = true;
        _model3D.enabled = false;
    }

    public void ShiftTo3D()
    {
        _render2D.enabled = false;
        _model3D.enabled = true;
    }


    public IEnumerator Hit(GameObject enemy, int damageTaken = 0)
    {
        if (!_isHit && _iFrames <= 0)
        {
            _rb.velocity = Vector2.zero;
            Debug.Log("Player Has Been Hit");
            _isHit = true;

            _iFrames = 2;

            Vector2 vec = new(transform.position.x - enemy.transform.position.x, 0);
            vec.Normalize();
            
            _rb.AddForce(new Vector2(vec.x, 1) * 10, ForceMode2D.Impulse);

            if (damageTaken > 0)
                Damage(damageTaken);

            yield return new WaitForSeconds(.2f);

            _rb.velocity = Vector3.zero;
        }
    }

    public void Damage(int amount)
    {

        if (this._currentHealth - amount >= 0)
        {
            this._currentHealth -= amount;
        }

        else
        {
            this._currentHealth = 0;
        }

    }

    public void AddManite(float value) {
        _currentManite += value;
        if (_currentManite >= _maxManite)
            _currentManite = _maxManite;
    }

    public void ReduceManite(float value) {
        _currentManite -= value;
        if (_currentManite <= 0)
            _currentManite = 0;
    }

    public void ObtainDash() {
        _hasDash = true;
    }
}
