using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    [SerializeField ]private float jumpHeight = 1.5f;
    [SerializeField] private float gravityOffset = 0;
    [SerializeField] private CameraMove cameraMove;
    private float verticalInput;
    private float horizontalInput;
    private Vector3 moveDirection;
    private CharacterController playerCharController;
    private float verticalVelocity;
   
    private float gravityValue;
    private bool grounded;



    void Start()
    {
        playerCharController = GetComponent<CharacterController>();
        gravityValue = -9.81f * gravityOffset;
    }
    void Update()
    {
        
        gravityValue = -9.81f * (gravityOffset + 1);
        grounded = playerCharController.isGrounded;
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        
        moveDirection = 
        transform.forward * verticalInput * moveSpeed * Time.deltaTime + 
        transform.right * horizontalInput * moveSpeed * Time.deltaTime;
        
        if(grounded == true)
        {
            verticalVelocity = -0.2f;
        } else {
            verticalVelocity += gravityValue * Time.deltaTime;
        }

        

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpHeight * -0.2f * gravityValue;
        }

        transform.rotation = Quaternion.Euler(0, cameraMove.currentX, 0);
        moveDirection.y = verticalVelocity;
        playerCharController.Move(moveDirection);
        Debug.Log(transform.rotation);
    }
}
