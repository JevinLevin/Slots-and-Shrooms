using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : StateMachine
{
    private CharacterController cc;

    [Header("References")]
    [SerializeField] private PlayerCamera playerCamera;
    public PlayerCamera GetCamera => playerCamera;

    [Header("Generic Settings")]
    [SerializeField] private float gravity;
    [SerializeField] private float playerWidth;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float baseFOV = 70;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float sensitivity = 5;
    [SerializeField] private float verticalClamp = 80;

    [Header("States")]
    public MovementSettings MovementSettings;
    public WalkingState WalkingState;
    public WalkingSettings WalkingSettings;
    public JumpingState JumpingState;
    public JumpingSettings JumpingSettings;
    public SprintingState SprintingState;
    public SprintingSettings SprintingSettings;

    private Vector2 cameraRotation;
    private Vector3 currentVelocity;
    public Vector3 GetVelocity => currentVelocity;
    private Vector3 externalVelocity;

    public bool IsGrounded => IsOnGround();

    protected override void Awake()
    {
        base.Awake();

        cc = GetComponent<CharacterController>();

        WalkingState = new WalkingState(this);
        JumpingState = new JumpingState(this);
        SprintingState = new SprintingState(this);

        currentState = WalkingState;
    }

    protected override void Start()
    {
        base.Start();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        cc.radius = playerWidth;

        playerCamera.SetFOV(baseFOV);
    }

    protected override void Update()
    {
        base.Update();

        CameraRotation();
        ApplyVelocity();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        cameraRotation.x += mouseX * sensitivity;
        cameraRotation.y += mouseY * sensitivity;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -verticalClamp, verticalClamp);

        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);

        cameraPivot.localRotation = xQuat * yQuat;
    }

    private void ApplyVelocity()
    {
        // Add gravity
        if (!cc.isGrounded)
            currentVelocity += Vector3.down * gravity * Time.deltaTime;

        Vector3 finalVelocity = currentVelocity + externalVelocity;
        MovePlayer(finalVelocity);

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
        return Physics.CheckSphere(transform.position + Vector3.down * 0.1f, playerWidth, environmentLayer);
    }
}
