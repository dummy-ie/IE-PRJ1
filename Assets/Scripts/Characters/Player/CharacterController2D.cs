 using Cinemachine;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour, ISaveable
{
#if UNITY_EDITOR
    [SerializeField] private bool _drawGizmos;
#endif
    Transform _lastSpawnPosition;
    public Transform LastSpawnPosition {
        get { return _lastSpawnPosition; }
        set { _lastSpawnPosition = value; }
    }

    [Header("Player Data")]

    [SerializeField]
    PlayerDataObject _dataObject;
    public PlayerDataObject DataObject
    {
        get { return _dataObject; }
    }

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

    [SerializeField]
    PlayerStatData _statData;
    public PlayerStatData StatsData
    {
        get { return _statData; }
    }

    [Header("Player Render")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _render2D;
    [SerializeField] MeshRenderer _model3D;

    [Header("Ground Check Box Cast")]
    [Range(0, 5)][SerializeField] private float _boxCastDistance = 0.4f;
    [SerializeField] Vector2 _boxSize = new(0.3f, 0.4f);

    private Rigidbody2D _rb;

    private CinemachineVirtualCamera _cmVC;
    private Cinemachine3rdPersonFollow _cmTP;
    private CinemachineConfiner2D _cmC2D;

    private float _deltaX = 0f;
    private float _deltaY = 0f;

    public float Vertical{ get { return _deltaY; } }

    private float _currentFallMultiplier;
    public float CurrentFallMultiplier
    {
        get { return _currentFallMultiplier; }
        set { _currentFallMultiplier = value; }
    }

    private float _coyoteTimeCounter = 0f, _jumpBufferCounter = 0f;
    
    private float _vectorShift = 100f;

    private int _isFacingRight = -1;

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
    [SerializeField]private bool _isGrounded = false;

    private bool _isHit = false;
    private float _setHitTime = .5f;
    private float _hitTime = .5f;

    private float _iFrames = 0;

    /*[SerializeField]
    private bool _hasDash = false;
    public bool HasDash
    {
        get { return _hasDash; }
        set { _hasDash = value; }
    }
    [SerializeField]
    private bool _hasSlash = false;
    public bool HasSlash
    {
        get { return _hasSlash; }
        set { _hasSlash = value; }
    }*/

    private float _dashCooldownTime = 0f;
    private float _dashSpeed = 0f;
    //private bool submitPressed = false;

    public int IsFacingRight {
        get { return _isFacingRight; }
    }
    public void FlipTo(int isFacingRight) { 
        _isFacingRight = isFacingRight;
    }
    private void Flip() {
        if (_isFacingRight == 1 && _deltaX < 0f || _isFacingRight == -1 && _deltaX > 0f)
        {
            _isFacingRight *= -1;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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
            Debug.Log("Jummp");
            _isJumpPress = true;
            // reset jump buffer counter
            _jumpBufferCounter = _dataObject.JumpBufferTime;
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
            _dashSpeed = _dataObject.DashOriginalSpeed;
            UpdateDashDuration();
            if (!_isDashing)
                AudioManager.Instance.PlaySFX(EClipIndex.MANITE_DASH);
            _isDashing = true;
            _aerialDash = false;

             //dash direction based on where playr is facing
             _dashSpeed *= _isFacingRight;
        }
    }
    private void Move()
    {
        if (!(_isDashing || _isHit || (DialogueManager.Instance != null && DialogueManager.Instance.IsPlaying)) && _canMove)
        {
            _rb.velocity = new Vector2(_deltaX * _dataObject.Speed, _rb.velocity.y);
        }

        // interpolate the camera towards where the player is currently moving if they are moving?
        // propose the idea later on idk
        //if (horizontal != 0) cmTP.CameraSide = Mathf.Lerp(cmTP.CameraSide, 0.5f * (horizontal + 1f), 0.05f);
    }

    private void Jump()
    {
        // while grounded, set coyote time and jump limit
        if (_isGrounded = IsGrounded())
        {
            if(!_extraJump)
                AudioManager.Instance.PlaySFX(EClipIndex.JUMP_LANDING);
            _coyoteTimeCounter = _dataObject.CoyoteTime;
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
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _dataObject.LowJumpMultiplier);
            _coyoteTimeCounter = 0f;
        }
        else if (_rb.velocity.y < 0f) // fast fall over time
        {
            _rb.velocity += (_currentFallMultiplier - 1f) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }

        // Jump if the buffer counter is active AND if coyote time is active OR you have an extra jump AND you can double jump
        if (_jumpBufferCounter > 0f && (_coyoteTimeCounter > 0f || (_extraJump && _dataObject.AllowDoubleJump)))
        {
            AudioManager.Instance.PlaySFX(EClipIndex.JUMP);
            _rb.velocity = new Vector2(_rb.velocity.x, _dataObject.JumpForce);
            _jumpBufferCounter = 0f;

            if (!_isGrounded && _coyoteTimeCounter <= 0f)
                _extraJump = false;
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

                _dashCooldownTime = _dataObject.DashCooldown; //set dash cooldown to max dashCooldon

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

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(Data.AttackCooldown);
        Data.CanAttack = true;
    }



    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _boxCastDistance, _dataObject.GroundLayer);
    }

    private void UpdateDashDuration()
    {
        _dashTime = _dashDuration = _dataObject.DashDistance / _dashSpeed;
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
        _cmC2D.m_BoundingShape2D = collider;
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

    private void Awake()
    {
        _cmVC = FindFirstObjectByType<CinemachineVirtualCamera>();
        _cmTP = _cmVC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _cmC2D = _cmVC.gameObject.GetComponent<CinemachineConfiner2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();


        //LOAD BUFFER
        //Every value that relies on PlayerData upon awake has to be placed under LoadBuffer function
        StartCoroutine(LoadBuffer());//TEMPORARY
        SetDataObjectToSaveable();
    }

    private void Start()
    {
        Debug.Log("Player");
        _stats.Health.SetMax(_dataObject.MaxHealth);
        _stats.Manite.SetMax(_dataObject.MaxManite);
    }

    private void Update()
    {

        Hits();

        if(this._stats.Health.Current == 0){
            PlayerSpawner.Instance.Respawn(Stats.CheckPointData.CheckPointName, Stats.CheckPointData.RespawnPosition);
            this._stats.Health.Current = this._dataObject.MaxHealth;
            this._dataObject.CanAttack = true;
        }
        Animate();

        Debug.Log(_stats.Manite.Current);

    }

    private void FixedUpdate() // move player on fixed update so collisions aren't fucky wucky
    {
        Move();
        Jump();
        Dash();
        
        if (!_isDashing) Flip();
    }

    //Load Buffer lets stuff load in first (eg. Awake func) before loading in data from file 
    public IEnumerator LoadBuffer() //NOTE: LOAD BUFFER HAS TO BE CALLED WHENEVER THE PLAYER CHOOSES TO CONTINUE A SAVE
    {
        yield return new WaitForSeconds(.1f);
        PlayerLoadData();

        SetSaveableToDataObject();

        _dashSpeed = _dataObject.DashOriginalSpeed;
        UpdateDashDuration();
    }

    //Data Saving
    public void LoadData(){}

    public void SaveData(){}

    public void PlayerLoadData()
    {
        this._data = JSONSave.Instance.LoadData<PlayerData>(this._data);
        this._statData = JSONSave.Instance.LoadData<PlayerStatData>(this._statData);
    }

    public void PlayerSaveData() //PlayerSaveData() ONLY TO BE CALLED IN CHECKPOINTS
    {
        SetDataObjectToSaveable();
        JSONSave.Instance.SaveData(this._data);
        JSONSave.Instance.SaveData(this._statData);
    }

    private void SetDataObjectToSaveable()
    {
        PropertyInfo[] dataObjProperties = typeof(PlayerDataObject).GetProperties();

        PropertyInfo[] dataProperties = typeof(PlayerData).GetProperties();

        //Debug.Log(dataObjProperties.Length + "  " + dataProperties.Length);


        for (int i = 0; i < dataObjProperties.Length - 2; i++)
        {
            dataProperties[i].SetValue(this._data, dataObjProperties[i].GetValue(this._dataObject, null));
        }

        this._statData.CheckPointData = this._stats.CheckPointData;

        this._statData.CurrentHealth = this._stats.Health.Current;

        this._statData.CurrentManite = this._stats.Manite.Current;

        this._statData.HasDash = this._stats.HasDash;
        this._statData.HasPound = this._stats.HasPound;
        this._statData.HasSlash = this._stats.HasSlash;
    }

    private void SetSaveableToDataObject()
    {

        PropertyInfo[] dataObjProperties = typeof(PlayerDataObject).GetProperties();

        PropertyInfo[] dataProperties = typeof(PlayerData).GetProperties();

        //Debug.Log(dataObjProperties.Length + "  " + dataProperties.Length);


        for (int i = 0; i < dataObjProperties.Length - 2;i++)
        {
            dataObjProperties[i].SetValue(this._dataObject, dataProperties[i].GetValue(this._data, null));
        }


        this._stats.CheckPointData = this._statData.CheckPointData;

        this._stats.Health.SetCurrent(this._statData.CurrentHealth);

        this._stats.Manite.SetCurrent(this._statData.CurrentManite);

        this._stats.HasDash = this._statData.HasDash;
        this._stats.HasPound = this._statData.HasPound;
        this._stats.HasSlash = this._statData.HasSlash;
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
    }
#endif
}
