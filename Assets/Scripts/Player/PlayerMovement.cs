using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //referance to camera to get rotation for the player 
    [SerializeField] private CameraMove cameraMove;
    //to check if player is holding a object
    private PlayerInteractController playerInteractController;

    //Player movement values
    //[Header("Movement values:")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float minCarrySpeedMultiplier = 0.01f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float minJumpMultiplier = 0.01f;
    //Desides how floaty the player is when airborn (Higher value means less floaty)
    [SerializeField] private float gravityStrenth = 6f;

    //WASD / Arrow keys input values
    private float verticalInput;
    private float horizontalInput;
    //Turns float values into velocity for the player
    private Vector3 moveDirection;
    private CharacterController playerCharController;
    //how fast the player is going up or down
    private float verticalVelocity;
    //how fast the player is going forwards
    private float currentForwardSpeed;
    //Default Gravity value 
    private float gravityValue = -9.81f;
    //true/false if the player is grounded
    private bool grounded;
    //coyote Time timer and time set
    [SerializeField] private float setCoyoteTime = 0.2f;
    private float coyoteTimer;
    //Jump buffer timer and time set
    [SerializeField] private float setJumpBuffer = 0.2f;
    private float jumpBuffer;
    //how long you can jump while holding spacebar
    [SerializeField] private bool chargeJump = true;
    [SerializeField] private float setJumpLength = 0.5f;
    private float jumpLength;
    private bool isJumping;
    //extra jumps
    [SerializeField] private int extraJumps = 1;
    private int remainingJumps;
    


    void Start()
    {
        //Grabs the Character Controller to manipulate it in the script
        playerCharController = GetComponent<CharacterController>();
        playerInteractController = GetComponent<PlayerInteractController>();
    }

    void Update()
    {
        GravityUpdate();
        MovementUpdate();
        JumpUpdate();
        SetVelocityUpdate();

        
    }

    void GravityUpdate()
    {
        //Checks if player is touching ground
        grounded = playerCharController.isGrounded;
        if(grounded)
        {
            //If grounded give weak gravity
            verticalVelocity = -2f;
            //sets coyote time to more then 0, to make jump work for a little after leaving the ground
            coyoteTimer = setCoyoteTime;
            
        }
        else 
        {
            //If not grounded give normal gravity over time
            verticalVelocity += gravityValue * gravityStrenth * Time.deltaTime;
        }
    }

    void MovementUpdate()
    {  
        //Get input from WASD and/or the arrow keys
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        
        
        //check if sprint key is being pressed
        if(verticalInput > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            //Set forwards speed to sprinting speed
            currentForwardSpeed = sprintSpeed;
        }
        else
        {
            //Set forwards speed to Walking speed
            currentForwardSpeed = walkSpeed;
        }

        // ===== NEW: weight slowdown =====
        float speedMultiplier = 1f;

        if (playerInteractController.IsHoldingObject)
        {
            float weightRatio = playerInteractController.HeldObject.WeightRatio(playerInteractController.playerStrength);
            // heavier object → slower movement
            speedMultiplier = Mathf.Lerp(1f, minCarrySpeedMultiplier, weightRatio);
        }

        currentForwardSpeed *= speedMultiplier;
        float strafeSpeed = walkSpeed * speedMultiplier;

        moveDirection = 
            transform.forward * verticalInput * currentForwardSpeed + 
            transform.right * horizontalInput * strafeSpeed;
    }

    void JumpUpdate()
    {
        //weight affects jump height
        float jumpMultiplier = 1f;

        if (playerInteractController.IsHoldingObject)
        {
            float weightRatio = playerInteractController.HeldObject.WeightRatio(playerInteractController.playerStrength);
            jumpMultiplier = Mathf.Lerp(1f, minJumpMultiplier, weightRatio);
        }

        //decrease coyote time over time
        if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBuffer = setJumpBuffer;
            jumpLength = 0f;
        }
        
        //makes sure that you can only jump while grounded
        if (jumpBuffer > 0f)
        {
            //decrease jump buffer over time
            jumpBuffer -= Time.deltaTime;
            //Normal jump
            if (coyoteTimer > 0f)
            {
                isJumping = true;
                //Set remaining Jumps to Extra jumps
                remainingJumps = extraJumps;
                //set coyote time and jump buffer to 0 to not get any extra jumps out of the same timer
                coyoteTimer = 0f;
                jumpBuffer = 0f;
            }
            //Double jump
            else if (remainingJumps > 0)
            {
                //Remove a jump
                remainingJumps -= 1;
                isJumping = true;
                //set jumpbuffer to 0
                jumpBuffer = 0f;
            }
            
        }
        
        if (isJumping == true)
        {
            if (chargeJump == true)
            {
                if (Input.GetKey(KeyCode.Space) && jumpLength < setJumpLength)
                {
                    jumpLength += Time.deltaTime;
                    //Set jump velocity
                    verticalVelocity = Mathf.Sqrt(Mathf.Max(0, jumpHeight * -1.8f * gravityValue * gravityStrenth * (setJumpLength + 0.5f - jumpLength)));
                    verticalVelocity = Mathf.Sqrt(Mathf.Max(0, jumpHeight * jumpMultiplier * -1.8f * gravityValue * gravityStrenth * (setJumpLength + 0.5f - jumpLength)));

                } 
                else
                {
                    //the timer ran out or the spacebar is no longer pressed
                    isJumping = false;
                }
            } 
            else
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * jumpMultiplier * -2f * gravityValue * gravityStrenth);
                isJumping = false;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if we hit something above us (normal points down)
        if (hit.normal.y < -0.9f && verticalVelocity > 0)
        {
            Debug.Log("Hit Head!");
            verticalVelocity = 0; // Stop upward movement
            isJumping = false;
        }
    }

    void SetVelocityUpdate()
    {
        //grabs the rotation of the camera and sets the player rotation to be the same so the movement of the player works with the camera direction
        transform.rotation = Quaternion.Euler(0, cameraMove.currentX, 0);
        
        //Normalize movement speed
        if (moveDirection.magnitude > currentForwardSpeed)
        {
            moveDirection = moveDirection.normalized * currentForwardSpeed;
        }
        //Turn moveDirection into velocity on the player
        moveDirection.y = verticalVelocity;

        moveDirection = moveDirection * transform.localScale.y;
        playerCharController.Move(moveDirection * Time.deltaTime);
    }

    
}
