using UnityEngine;

public class Drawer : MonoBehaviour
{
    
    [SerializeField] private LayerMask interactionLayers;

    private Animator mAnimator;
    private bool isOpen = false;

    [Header("Referance")]
    [SerializeField] private PlayerInteractController playerInteractController;
    public Camera playerCamera;
    
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, playerInteractController.pickupDistance, interactionLayers))
            {
                if (hit.transform == transform)
                {
                    if (isOpen == false)
                    {
                        mAnimator.SetTrigger("Open");
                        isOpen = true;
                        Debug.Log("Open Draw");
                    }
                    else if (isOpen == true)
                    {
                        mAnimator.SetTrigger("Close");
                        isOpen = false;
                        Debug.Log("Close Draw");
                    }
                }
            }
        }
    }
}
