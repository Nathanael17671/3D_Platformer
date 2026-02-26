using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;  
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance = 5f;
    [SerializeField] private GameObject pickupText;

    private GrabbableObject grabbableObject;

    void Update()
    {
        Pickup();
    }

    void Pickup()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            if (grabbableObject == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out grabbableObject))
                    {
                        grabbableObject.Grab(objectGrabPointTransform);
                        Debug.Log(grabbableObject);
                    }
                }
            }
        }  
        
        if (Input.GetMouseButtonUp(1))
        {
            grabbableObject.Drop();
            grabbableObject = null;
        }
        
    }
}
