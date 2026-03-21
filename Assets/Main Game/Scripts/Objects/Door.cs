using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private PlayerInteractController playerInteractController;
    [SerializeField] private LayerMask interactionLayers;

    [Header("Hinge")]
    public float closedPosition = 0f;
    public float openPosition = 90f;
    public float openStrength = 100f;
    public float doorDamper = 15f;

    private HingeJoint hingeJoint;
    private Rigidbody rigidbody;
    JointSpring spring;
    private bool isOpen = false;

    [Header("Referance")]
    public Camera playerCamera;
    
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();
        hingeJoint.useSpring = true;
        spring = hingeJoint.spring;
    }

    // Update is called once per frame
    void Update()
    {
        spring.spring = openStrength;
        spring.damper = doorDamper;
        hingeJoint.useLimits = true;
    
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, playerInteractController.pickupDistance, interactionLayers))
            {
                if (hit.transform == transform)
                {
                    if (isOpen == false)
                    {
                        spring.targetPosition = openPosition;
                        isOpen = true;
                        hingeJoint.spring = spring;
                        Debug.Log("Open Door");
                    }
                    else if (isOpen == true)
                    {
                        spring.targetPosition = closedPosition;
                        isOpen = false;
                        hingeJoint.spring = spring;
                        Debug.Log("Close Door");
                    }
                }
            }
        }
    }
}
