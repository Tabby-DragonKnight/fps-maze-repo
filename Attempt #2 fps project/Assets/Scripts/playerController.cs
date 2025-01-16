using UnityEngine;

public class playerController : MonoBehaviour
{

    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] float speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpmax;
    [SerializeField] int jumpspeed;
    [SerializeField] int gravity;
    Vector3 moveDir;
    Vector3 playerVel;
    private bool isCrouched = false;
    public float m_baseSpeed = 0.0f;
    bool isSprinting;
    int jumpCount;
    [SerializeField] private int health = 10;





    [Header("Camera Settings")]
    public Camera playerCamera; // Reference to the player camera
    public float crouchCameraHeight; // Camera Height when crouched
    private float originalCameraHeight; // Original camera's height

    [Header("Collider Settings")]
    private CapsuleCollider playerCollider;
    public float crouchColliderHeight = 1.0f;
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;

    [Header("Crouching")]
    [SerializeField]
    private float crouchSpeed = 3.5f;
    [SerializeField]
    private float crouchMoveSpeed = 5.0f;
    [SerializeField]
    private float crouchYScale = .5f;

    [Header("Keybinds")]
    public KeyCode crouchKey = KeyCode.LeftControl;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalCameraHeight = playerCamera.transform.localPosition.y;
        playerCollider = GetComponent<CapsuleCollider>();
        originalColliderHeight = playerCollider.height;
        originalColliderCenter = playerCollider.center;

        m_baseSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
       
        movement();
        sprint();
        Crouch();

    }

    void movement()
    {

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }


        // moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.position += moveDir * speed * Time.deltaTime;

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                  (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();
        Crouch();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

       
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && !isCrouched)
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && !isCrouched)
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpmax)
        {
            jumpCount++;
            playerVel.y = jumpspeed;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            speed = crouchMoveSpeed;
            isCrouched = true;

            playerCollider.height = crouchColliderHeight;
            playerCollider.center = new Vector3(playerCollider.center.x, playerCollider.center.y / 2, playerCollider.center.z);

            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, crouchCameraHeight, playerCamera.transform.localPosition.z);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            speed = m_baseSpeed;
            isCrouched = false;

            playerCollider.height = originalColliderHeight;
            playerCollider.center = originalColliderCenter;

            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalCameraHeight, playerCamera.transform.localPosition.z);
        }
    }


}
