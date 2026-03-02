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

    private GrabbableObject heldObject;
    private GrabbableObject hoveredObject;
    private Collider heldCollider;

    public bool IsHoldingObject => heldObject != null;
    public GrabbableObject HeldObject => heldObject;

    void Update()
    {
        DetectHover();
        HandlePickup();
        UpdateCameraWeight();
    }

    void DetectHover()
    {
        hoveredObject = null;

        if (Physics.Raycast(
            playerCameraTransform.position,
            playerCameraTransform.forward,
            out RaycastHit hit,
            pickupDistance,
            pickupLayerMask))
        {
            hit.transform.TryGetComponent(out hoveredObject);
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

                    //remember collider and ignore collision
                    heldCollider = heldObject.GetComponent<Collider>();

                    Collider playerCol = GetComponent<CharacterController>();
                    Physics.IgnoreCollision(playerCol, heldCollider, true);
                }
            }
        }

        if (Input.GetMouseButtonUp(1) && heldObject != null)
        {
            heldObject.Drop();

            //restore collision
            if (heldCollider != null)
            {
                Collider playerCol = GetComponent<CharacterController>();
                Physics.IgnoreCollision(playerCol, heldCollider, false);
            }

            heldObject = null;
            heldCollider = null;
        }
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