using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : StateMachine
{
    private CharacterController cc;

    [Header("References")] 
    [SerializeField] private PlayerStatsHolderSO playerStats;
    [SerializeField] private PlayerCamera playerCamera;
    public PlayerCamera GetCamera => playerCamera;
    [SerializeField] private PlayerShooter playerShooter;
    public PlayerShooter PlayerShooter => playerShooter;
    [SerializeField] private PlayerAnimator playerAnimator;
    public PlayerAnimator PlayerAnimator => playerAnimator;

    [Header("Generic Settings")]
    [SerializeField] private float gravity;
    [SerializeField] private float playerWidth;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float baseFOV = 70;
    [SerializeField] private bool disableSprinting;
    [SerializeField] private bool disableJumping;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity = 5;
    [SerializeField] private float verticalClamp = 80;
    [SerializeField] private Transform legsPivot;
    public Transform GetLegsPivot => legsPivot;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip walkingAudio;
    public AudioClip WalkingAudio => walkingAudio;

    [SerializeField] private AudioClip jumpingAudio;
    public AudioClip JumpingAudio => jumpingAudio;

    [SerializeField] private AudioClip slidingAudio;
    public AudioClip SLidingAudio => slidingAudio;

    [Header("States")]

    public MovementSettings MovementSettings;
    public IdleState IdleState;
    public IdleSettings IdleSettings;
    public WalkingState WalkingState;
    public WalkingSettings WalkingSettings;
    public JumpingState JumpingState;
    public JumpingSettings JumpingSettings;
    public SprintingState SprintingState;
    public SprintingSettings SprintingSettings;
    public CrouchingState CrouchingState;
    public CrouchingSettings CrouchingSettings;
    public SlidingState SlidingState;
    public SlidingSettings SlidingSettings;

    private Vector2 cameraRotation;
    private Vector3 currentVelocity;
    public Vector3 GetVelocity => currentVelocity;
    private Vector3 externalVelocity;
    private bool cameraLocked;

    public bool IsGrounded => IsOnGround();
    public bool IsMoving => Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0 || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0;
    public bool IsHoldingSprint => Input.GetKey(KeyCode.LeftShift);
    public bool IsPressingSprint => Input.GetKey(KeyCode.LeftShift);
    public bool IsHoldingCrouch => Input.GetKey(KeyCode.LeftControl);
    public bool IsPressingCrouch => Input.GetKeyDown(KeyCode.LeftControl);
    public bool IsHoldingJump => Input.GetKey(KeyCode.Space);
    public float BasePlayerWidth => playerWidth;
    public float BasePlayerHeight => playerHeight;
    public bool DisableSprinting => disableSprinting;
    public bool DisableJumping => disableJumping;

    public float BaseSpeedMultiplier => playerStats.GetStatAsMultiplier(StatType.Speed);
    public float JumpHeightMultiplier => playerStats.GetStatAsMultiplier(StatType.JumpHeight);
    public float SprintSpeedMultiplier => playerStats.GetStatAsMultiplier(StatType.SprintSpeedMultiplier);
    public float CrouchSpeedMultiplier => playerStats.GetStatAsMultiplier(StatType.CrouchSpeed);
    public float SlideSpeedMultiplier => playerStats.GetStatAsMultiplier(StatType.SlideSpeed);
    public float SlideDurationMultiplier => playerStats.GetStatAsMultiplier(StatType.SlideDuration);
    public float GravityMultiplier => playerStats.GetStatAsMultiplier(StatType.Gravity);
    public float AimingMultipler => playerStats.GetStatAsMultiplier(StatType.SpeedWhileAiming);

    protected override void Awake()
    {
        base.Awake();

        cc = GetComponent<CharacterController>();

        IdleState = new IdleState(this);
        WalkingState = new WalkingState(this);
        JumpingState = new JumpingState(this);
        SprintingState = new SprintingState(this);
        CrouchingState = new CrouchingState(this);
        SlidingState = new SlidingState(this);

        currentState = IdleState;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += OnDie;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= OnDie;
    }

    protected override void Start()
    {
        base.Start();

        ResetWidth();
        ResetHeight();
        cameraRotation.x = 90;
        cameraRotation.y = -25;

        playerCamera.SetFOV(baseFOV);

    }

    protected override void Update()
    {
        base.Update();

        ApplyRotation();
        ApplyVelocity();
    }

    private void OnDie()
    {
        playerAnimator.enabled = false;
        playerShooter.enabled = false;
        enabled = false;
        LockCamera(true, 0f);
    }

    private void LockCamera(bool value, float verticalAngle)
    {
        cameraLocked = value;
        cameraPivot.localEulerAngles = new Vector3(verticalAngle, cameraPivot.localEulerAngles.y, cameraPivot.localEulerAngles.z);
    }

    private void ApplyRotation()
    {
        if (cameraLocked)
            return;

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;
        cameraRotation.x += mouseX * sensitivity;
        cameraRotation.y += mouseY * sensitivity;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -verticalClamp, verticalClamp);

        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);
 
        // Rotate camera vertically
        cameraPivot.localRotation = Quaternion.Euler(Vector3.up) * yQuat;
        // Rotate player horizontally
        transform.localRotation = Quaternion.Euler(Vector3.left) * xQuat;
    }

    private void ApplyVelocity()
    {
        // Add gravity
        if (!cc.isGrounded)
            currentVelocity += Vector3.down * (gravity * Time.deltaTime * GravityMultiplier);

        Vector3 finalVelocity = currentVelocity + externalVelocity;
        MovePlayer(finalVelocity * Time.deltaTime);

        externalVelocity = Vector3.zero;


        if (cc.isGrounded)
            currentVelocity = new(currentVelocity.x, 0, currentVelocity.z);


    }

    public void SetVelocity(Vector3 velocity)
    {
        currentVelocity = velocity;
    }
    public void ImpulseVelocity(Vector3 velocity)
    {
        currentVelocity += velocity;
    }
    public void SetExternalVelocity(Vector3 velocity)
    {
        externalVelocity = velocity;
    }

    private void MovePlayer(Vector3 offset)
    {
        cc.Move(offset);
    }

    public void TeleportPlayer(Vector3 position)
    {
        cc.enabled = false;
        transform.position = position;
        cc.enabled = true;
    }

    protected override void OnStateSwitched()
    {

    }

    private bool IsOnGround()
    {
        return CheckSphere((playerWidth/2) -0.1f);
    }

    public bool CheckSphere(float yOffset)
    {
        return Physics.CheckSphere(transform.position + Vector3.up * yOffset, playerWidth, environmentLayer);
    }


    public void SetWidth(float width)
    {
        cc.radius = width;
    }
    public void ResetWidth()
    {
        SetWidth(playerWidth);
    }
    public void SetHeight(float height)
    {
        cc.height = height;
        cc.center = new(0, height/2, 0);
    }
    public void ResetHeight()
    {
        SetHeight(playerHeight);
    }
    
}
