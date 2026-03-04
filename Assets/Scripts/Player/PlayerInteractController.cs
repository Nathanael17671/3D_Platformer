using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private CameraMove cameraMove;

    [Header("Strength")]
    public float playerStrength = 10f;

    [Header("Pickup")]
    [SerializeField] public float pickupDistance = 5f;

    [Header("Cursor")]
    [SerializeField] private GameObject normalCursor;
    [SerializeField] private GameObject grabCursor;
    [SerializeField] private GameObject heavyCursor;

    private Collider playerCollider;
    private GrabbableObject heldObject;
    private GrabbableObject hoveredObject;
    private Collider[] heldCollider;

    public bool IsHoldingObject => heldObject != null;
    public GrabbableObject HeldObject => heldObject;

    void Awake()
    {
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        DetectHover();
        HandlePickup();
        UpdateCameraWeight();
    }

    void DetectHover()
    {
        GrabbableObject newHover = null;

        if (Physics.Raycast(
            playerCameraTransform.position,
            playerCameraTransform.forward,
            out RaycastHit hit,
            pickupDistance,
            pickupLayerMask))
        {
            hit.transform.TryGetComponent(out newHover);
        }

        // only change if different object
        if (hoveredObject != newHover)
        {
            if (hoveredObject != null && hoveredObject != heldObject)
                hoveredObject.SetHover(false, playerCameraTransform);

            hoveredObject = newHover;

            if (hoveredObject != null)
                hoveredObject.SetHover(true, playerCameraTransform);
        }

        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (hoveredObject == null)
        {
            normalCursor.SetActive(true);
            heavyCursor.SetActive(false);
            grabCursor.SetActive(false);
            return;
        }

        if (hoveredObject.TooHeavyToLift(playerStrength))
        {
            normalCursor.SetActive(false);
            heavyCursor.SetActive(true);
            grabCursor.SetActive(false);
        }
        else
        {
            normalCursor.SetActive(false);
            heavyCursor.SetActive(false);
            grabCursor.SetActive(true);
        }
            
    }

    void HandlePickup()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (heldObject == null && hoveredObject != null)
            {
                if (!hoveredObject.TooHeavyToLift(playerStrength))
                {
                    heldObject = hoveredObject;
                    heldObject.Grab(objectGrabPointTransform, playerStrength);
                    heldObject.SetHover(true, playerCameraTransform);

                    //remember collider and ignore collision
                    heldCollider = heldObject.GetComponentsInChildren<Collider>();

                    Collider playerCol = GetComponent<CharacterController>();
                    foreach (var col in heldCollider)
                    {
                        Physics.IgnoreCollision(playerCol, col, true);
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonUp(1) && heldObject != null)
        {
            heldObject.Drop();
            heldObject.SetHover(false, playerCameraTransform);

            Collider playerCol = GetComponent<CharacterController>();
            //restore collision
            if (heldCollider != null)
            {
                foreach (var col in heldCollider)
                {
                    Physics.IgnoreCollision(playerCol, col, true);
                }
            }

            heldObject = null;
            heldCollider = null;
        }
        if (heldObject != null)
        {
            heldObject.objectPlayerStrength = playerStrength;
        }
    }

    public GrabbableObject GetHeldObject()
    {
        return heldObject;
    }

    // camera slows when heavy object held
    void UpdateCameraWeight()
    {
        if (heldObject == null)
        {
            cameraMove.SetWeightMultiplier(1f);
            return;
        }

        float ratio = heldObject.WeightRatio(playerStrength);
        float multiplier = Mathf.Lerp(1f, 0.2f, ratio);

        cameraMove.SetWeightMultiplier(multiplier);
    }
}