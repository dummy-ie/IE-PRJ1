using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour, ISaveable
{
#if UNITY_EDITOR
    [SerializeField] private bool _drawGizmos;
#endif
    // INPUTS
    private StuckinBetween _playerActions;

    private InputAction _moveAction;

    CharacterSpawnPoint _lastSpawnPosition;
    public CharacterSpawnPoint LastSpawnPosition
    {
        get { return _lastSpawnPosition; }
        set { _lastSpawnPosition = value; }
    }

    [Header("Player Data")]


    [SerializeField]
    PlayerData _data;
    public PlayerData Data
    {
        get { return _data; }
    }


    [SerializeField]
    PlayerStatField _stats;
    public PlayerStatField Stats
    {
        get { return _stats; }
    }

    [Header("Player Render")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _render2D;
    [SerializeField] MeshRenderer _model3D;
    [SerializeField] private Material _flashMaterial;

    private Material _original2DMaterial;
    private Material _original3DMaterial;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float _boxCastDistance = 0.4f;
    [SerializeField] Vector2 _boxSize = new(0.3f, 0.4f);

    private Rigidbody2D _rb;

    private float _deltaX = 0f;
    private float _deltaY = 0f;

    public float Vertical { get { return _deltaY; } }

    private bool _isPressDown;

    private float _currentFallMultiplier;
    public float CurrentFallMultiplier
    {
        get { return _currentFallMultiplier; }
        set { _currentFallMultiplier = value; }
    }

    private float _coyoteTimeCounter = 0f, _jumpBufferCounter = 0f;

    private float _vectorShift = 100f;

    private int _facingDirection = -1;

    private bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    private bool _isDashing = false;
    private float _dashTime = 0f;
    private float _dashDuration;
    private bool _aerialDash = true;
    private bool _canDash = true;

    private bool _isJumpPress = false;
    private bool _extraJump = false;
    [SerializeField] private bool _isGrounded = false;

    private bool _isHit = false;
    private float _setHitTime = .5f;
    private float _hitTime = .5f;

    private float _iFrames = 0;

    private float _dashCooldownTime = 0f;
    private float _dashSpeed = 0f;
    //private bool submitPressed = false;

    private bool _isInvisible = false;

    public bool IsInvisible
    {
        get { return _isInvisible; }
    }
    private float _invisibilityTicks = 0.0f;
    public float InvisibilityTime = 1.0f;

    private bool _onLadder = false;
    public bool OnLadder
    {
        set { _onLadder = value; }
    }

    public int FacingDirection
    {
        get { return _facingDirection; }
    }
    public void FlipTo(int facingDirection)
    {
        _facingDirection = facingDirection;
    }
    private void Flip()
    {
        if (_deltaX < 0f) 
            FlipTo(1);
        else if (_deltaX > 0f)
            FlipTo(-1);
        //if (_facingDirection == 1 && _deltaX < 0f || _facingDirection == -1 && _deltaX > 0f)
        //{
            //_facingDirection *= -1;
            Vector3 localScale = transform.localScale;
            localScale.x = _facingDirection;
            transform.localScale = localScale;
        //}
    }

    private OneWayPlatform GetPlatformBelow()
    {
        OneWayPlatform platform = null;
        Collider2D collider = GetGroundRaycast().collider;
        if (collider != null)
            platform = collider.gameObject.GetComponent<OneWayPlatform>();
        return platform;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
        Debug.Log("Jummp");
        if (context.started)
        {
            OneWayPlatform platform = GetPlatformBelow();
            if (platform != null && _deltaY <= -0.5f)
            {
                Debug.Log("SHOULD DROP DOWN");
                platform.StartDropPlatform();
                return;
            }

            _isJumpPress = true;
            // reset jump buffer counter
            _jumpBufferCounter = _data.JumpBufferTime;
        }
        else if (context.canceled)
        {
            _isJumpPress = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && _canDash && _stats.HasDash) //check if player can dash
        {
            _dashSpeed = _data.DashOriginalSpeed;
            UpdateDashDuration();
            if (!_isDashing)
                AudioManager.Instance.PlaySFX(EClipIndex.MANITE_DASH);
            _isDashing = true;
            _aerialDash = false;

            //dash direction based on where playr is facing
            _dashSpeed *= -_facingDirection;
        }
    }
    private void Move()
    {
        if (_moveAction != null)
        {
            _deltaX = _moveAction.ReadValue<Vector2>().x;
            _deltaY = _moveAction.ReadValue<Vector2>().y;

            if (_canMove)
            {
                if (!(_isDashing || _isHit || (DialogueManager.Instance != null && DialogueManager.Instance.IsPlaying)))
                    _rb.velocity = new Vector2(_deltaX * _data.Speed, _rb.velocity.y);
                if (_onLadder)
                    _rb.velocity = new Vector2(_rb.velocity.x, _deltaY * _data.Speed);
            }
            // interpolate the camera towards where the player is currently moving if they are moving?
            // propose the idea later on idk
            //if (horizontal != 0) cmTP.CameraSide = Mathf.Lerp(cmTP.CameraSide, 0.5f * (horizontal + 1f), 0.05f);
        }
    }

    private void Jump()
    {
        // checks if player is not pressing down
        if (_deltaY > -0.5f)
        {
            // while grounded, set coyote time and jump limit
            if (_isGrounded = IsGrounded())
            {
                if (!_extraJump)
                    AudioManager.Instance.PlaySFX(EClipIndex.JUMP_LANDING);
                _coyoteTimeCounter = _data.CoyoteTime;
                _extraJump = true; // refresh double jump
                _aerialDash = true; //refresh aerialDash
            }
            else
                _coyoteTimeCounter -= Time.deltaTime; // otherwise, count down the coyote time

            if (!IsGrounded())
                _jumpBufferCounter -= Time.deltaTime; // when not pressing jump, count down jump buffering time

            if (!_isJumpPress && !_onLadder && _rb.velocity.y > 0f)
            {
                // fall when releasing the jump button early (allows low jumping)
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _data.LowJumpMultiplier);
                _coyoteTimeCounter = 0f;
            }
            else if (_rb.velocity.y < 0f) // fast fall over time
            {
                _rb.velocity += (_currentFallMultiplier - 1f) * Time.deltaTime * Physics2D.gravity * Vector2.up;
            }

            // Jump if the buffer counter is active AND if coyote time is active OR you have an extra jump AND you can double jump
            if (_jumpBufferCounter > 0f && (_coyoteTimeCounter > 0f || (_extraJump && _data.AllowDoubleJump)))
            {
                AudioManager.Instance.PlaySFX(EClipIndex.JUMP);
                _rb.velocity = new Vector2(_rb.velocity.x, _data.JumpForce);
                _jumpBufferCounter = 0f;

                if (!_isGrounded && _coyoteTimeCounter <= 0f)
                    _extraJump = false;
            }
        }

    }


    private void Dash()
    {
        if (_stats.HasDash)
        {
            if (_isDashing)
            {
                _dashTime -= Time.deltaTime;// dash duration countdown

                _rb.velocity = new Vector2(_dashSpeed, 0); // the actual dashing code 

                _dashCooldownTime = _data.DashCooldown; //set dash cooldown to max dashCooldon

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

    private void Animate()
    {
        _animator.SetBool("IsGrounded", IsGrounded());
        bool isRunning = _rb.velocity.x > 0 || _rb.velocity.x < 0;
        _animator.SetBool("IsRunning", isRunning);
        _animator.SetFloat("Y-axis Speed", _rb.velocity.y);
        if (!IsGrounded())
        {
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

    public void StartCooldown(float cooldown)
    {
        StartCoroutine(Cooldown(cooldown));
    }

    private IEnumerator Cooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        Data.CanAttack = true;
    }


    private RaycastHit2D GetGroundRaycast()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _data.GroundLayer);
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _data.GroundLayer);
    }

    private void UpdateDashDuration()
    {
        _dashTime = _dashDuration = _data.DashDistance / _dashSpeed;
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

    public void StartHit(HitData hitData)
    {
        StartCoroutine(Hit(hitData));
    }

    public void StartBlink()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Hit(HitData hitData)
    {
        if (!_isHit && _iFrames <= 0)
        {
            _rb.velocity = Vector2.zero;
            Debug.Log("Player Has Been Hit");
            _isHit = true;
            _iFrames = 1;

            StartBlink();

            //Vector2 vec = new(transform.position.x - enemy.transform.position.x, 0);
            //vec.Normalize();

            _rb.AddForce(hitData.force, ForceMode2D.Impulse);

            if (hitData.damage > 0)
                Damage((int)hitData.damage);

            yield return new WaitForSeconds(.2f);

            _rb.velocity = Vector3.zero;
        }
    }

    private IEnumerator Blink()
    {
            Debug.Log("Is blinking");
            _render2D.material = _flashMaterial;
            _model3D.material = _flashMaterial;

            yield return new WaitForSeconds(0.3f);

            _render2D.material = _original2DMaterial;
            _model3D.material = _original3DMaterial;

            yield return new WaitForSeconds(0.3f);
    }

    public void Damage(int amount)
    {

        if (this._stats.Health.Current - amount >= 0)
        {
            this._stats.Health.Current -= amount;
        }

        else
        {
            this._stats.Health.Current = 0;
        }

    }

    public void SetVirtualCameraBoundingBox(Collider2D collider)
    {
        CinemachineConfiner2D cmC2D = CameraUtility.playerVCam.gameObject.GetComponent<CinemachineConfiner2D>();
        cmC2D.m_BoundingShape2D = collider;
    }

    public void ResetFallMultiplier()
    {
        _currentFallMultiplier = Data.FallMultiplier;
    }

    public void ObtainDash()
    {
        _stats.HasDash = true;
    }

    public void ObtainSlash()
    {
        _stats.HasSlash = true;
    }

    public void ObtainGroundPound()
    {
        _stats.HasPound = true;
    }

    public void ObtainInvisibility()
    {
        _stats.HasInvisibility = true;
    }

    public void ActivateInvisible()
    {
        _isInvisible = true;
    }

    public void DeactivateInvisible()
    {
        _isInvisible = false;
    }

    public void OnPressInvisibility(InputAction.CallbackContext context)
    {
        if (_stats.HasInvisibility)
        {
            _isInvisible = !_isInvisible;
        }
    }

    private CharacterSpawnPoint GetSpawnPoint(SceneLoader.TransitionData transitionData)
    {

        SpawnPoints spawnPoints = transitionData.currentScene.spawnPoints;
        SpawnPoints.SpawnPoint spawnPoint = spawnPoints.defaultSpawnPoint;

        spawnPoints.TryGetSpawnPoint(transitionData.spawnPoint, ref spawnPoint);

        Debug.Log("Spawn Point Position : " + spawnPoint.position);

        return new CharacterSpawnPoint()
        {
            position = spawnPoint.position,
            faceToRight = spawnPoint.faceToRight
        };
    }

    public void RespawnOnCheckpoint()
    {
        if (_stats.CheckPointData.CheckPointName == "default")
        {
            RespawnOnLastSpawnPoint();
            return;
        }
        CharacterSpawnPoint checkpointSpawn = new CharacterSpawnPoint()
        {
            position = new Vector2(_stats.CheckPointData.PosX, _stats.CheckPointData.PosY),
        };
        transform.position = checkpointSpawn.position;


        // TODO : LOAD SCENE IF DIFFERENT SCENE REFERENCE OTHERWISE RESPAWN PLAYER

        //SceneLoader.Instance.LoadSceneWithFade(_stats.CheckPointData.);
    }

    public void RespawnOnLastSpawnPoint()
    {
        transform.position = _lastSpawnPosition.position;
        FlipTo(_lastSpawnPosition.faceToRight ? 1 : -1);
    }

    public void OnSceneLoad(SceneLoader.TransitionData transitionData)
    {
        _lastSpawnPosition = GetSpawnPoint(transitionData);
        transform.position = _lastSpawnPosition.position;
        FlipTo(_lastSpawnPosition.faceToRight ? -1 : 1);
    }
        
    private void Awake()
    {
        _playerActions = new StuckinBetween();

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _stats.Health.SetMax(_data.MaxHealth);
        _stats.Manite.SetMax(_data.MaxManite);
        _stats.Health.SetCurrent(_data.MaxHealth);
        _stats.Manite.SetCurrent(_data.MaxManite);

        _original2DMaterial = _render2D.material;
        _original3DMaterial = _model3D.material;

        StartCoroutine(LoadBuffer());
    }

    private void Update()
    {

        Hits();

        if (_isInvisible)
        {
            _invisibilityTicks += Time.deltaTime;

            Color color = _render2D.color;
            color.a = 0.5f;
            _render2D.color = color;

            if (_invisibilityTicks >= InvisibilityTime)
            {
                _stats.Manite.Current -= 1;
                _invisibilityTicks = 0.0f;
            }
            
        }
        else
        {
            Color color = _render2D.color;
            color.a = 1f;
            _render2D.color = color;
        }
            

        if (this._stats.Health.Current <= 0)
        {
            RespawnOnCheckpoint();
            this._stats.Health.Current = this._data.MaxHealth;
            this._data.CanAttack = true;
            //Destroy(gameObject);
        }

        if (transform.position.y <= -250)
            RespawnOnLastSpawnPoint();
        Animate();

    }

    private void FixedUpdate() // move player on fixed update so collisions aren't fucky wucky
    {
        Move();
        Jump();
        Dash();
        if (_onLadder)
            _rb.gravityScale = 0;
        else
        {
            _rb.gravityScale = 3;
        }
        if (!_isDashing) Flip();
    }

    void OnEnable()
    {

        if (_playerActions == null)
        {
            _playerActions = new StuckinBetween();

            //inputActions.Player.SetCallbacks(this);
        }
        Debug.Log(_playerActions + " PLAYER ENABLED");

        if (HUDManager.Instance != null)
        {
            _stats.Health.CurrentChanged += HUDManager.Instance.SetHearts;
            _stats.Manite.CurrentChanged += HUDManager.Instance.SetManiteValue;
        }
            
        _moveAction = _playerActions.Player.Move;
        _moveAction.Enable();

        _playerActions.Player.Pause.started += PauseManager.Instance.OnPauseGame;
        _playerActions.Player.Pause.Enable();

        _playerActions.Player.Jump.started += OnJump;
        _playerActions.Player.Jump.canceled += OnJump;
        _playerActions.Player.Jump.Enable();

        _playerActions.Player.Dash.started += OnDash;
        _playerActions.Player.Dash.Enable();

        _playerActions.Player.Attack.started += GetComponent<PlayerAttack>().OnAttack;
        _playerActions.Player.Attack.Enable();

        _playerActions.Player.Interact.started += GetComponent<PlayerInteract>().OnInteract;
        _playerActions.Player.Interact.Enable();

        //_playerActions.Player.ManiteSlash.started += GetComponent<ManiteSlash>().OnManiteSlash;
        //_playerActions.Player.ManiteSlash.Enable();

        _playerActions.Player.ChargedThrust.started += GetComponent<ChargedThrust>().OnChargedThrust;
        _playerActions.Player.ChargedThrust.Enable();

        _playerActions.Player.GroundPound.started += GetComponent<GroundPound>().OnGroundPound;
        _playerActions.Player.GroundPound.Enable();

        _playerActions.Player.Ability1.started += OnPressInvisibility;
        _playerActions.Player.Ability1.Enable();
    }

    void OnDisable()
    {
        _stats.Health.CurrentChanged -= HUDManager.Instance.SetHearts;
        _stats.Manite.CurrentChanged -= HUDManager.Instance.SetManiteValue;

        _moveAction.Disable();

        _playerActions.Player.Jump.started -= OnJump;
        _playerActions.Player.Jump.canceled -= OnJump;
        _playerActions.Player.Jump.Disable();

        _playerActions.Player.Dash.started -= OnDash;
        _playerActions.Player.Dash.Disable();

        _playerActions.Player.Attack.started -= GetComponent<PlayerAttack>().OnAttack;
        _playerActions.Player.Attack.Disable();

        _playerActions.Player.Interact.started -= GetComponent<PlayerInteract>().OnInteract;
        _playerActions.Player.Interact.Disable();

        //_playerActions.Player.ManiteSlash.started -= GetComponent<ManiteSlash>().OnManiteSlash;
        //_playerActions.Player.ManiteSlash.Disable();

        _playerActions.Player.GroundPound.started -= GetComponent<GroundPound>().OnGroundPound;
        _playerActions.Player.GroundPound.Disable();

        _playerActions.Player.Ability1.started -= OnPressInvisibility;
        _playerActions.Player.Ability1.Disable();
    }

    private IEnumerator LoadBuffer()
    {
        yield return new WaitForSeconds(.1f);
        LoadData();

        _dashSpeed = _data.DashOriginalSpeed;
        UpdateDashDuration();
    }

    public void LoadData()
    {
        //JSONSave.Instance.LoadData<PlayerData>(ref this._data);
        JSONSave.Instance.LoadData<PlayerStatField>(ref this._stats);
    }

    public void SaveData()
    {
        //JSONSave.Instance.SaveData(this._data);
        JSONSave.Instance.SaveData(this._stats);
    }


    /*Experimental SHit
     * 
     * public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }*/

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_drawGizmos)
            return;
        Gizmos.DrawWireCube(transform.position - Vector3.up * _boxCastDistance, _boxSize);
        DrawAttack(_data.FirstAttack);

        void DrawAttack(PlayerData.Attack attackData)
        {
            Vector3 facing = Vector3.right * _facingDirection;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + facing * attackData.HitboxCastDistance, attackData.HitboxSize);
        }
    }
#endif
}
