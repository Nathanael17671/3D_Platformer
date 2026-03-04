using UnityEngine;
using TMPro;

public class ProximityScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private GameObject proximityDisplay;

    [Header("Hover Text")]
    [SerializeField] private GameObject hoverTextRoot;
    [SerializeField] private TextMeshProUGUI hoverText;
    [SerializeField] private string objectName = "Object";

    private Transform lookTarget;

    void Start()
    {
        proximityDisplay.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            proximityDisplay.SetActive(true);
            SetHover(true, playerCameraTransform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            proximityDisplay.SetActive(false);
            SetHover(false, playerCameraTransform);
        }
    }

    void Update()
    {
        
    }


    public void SetHover(bool state, Transform cameraTransform)
    {

        if (hoverTextRoot == null) return;

        hoverTextRoot.SetActive(state);

        if (state)
        {
            lookTarget = cameraTransform;

            
            hoverText.text = objectName;
    
        }
        else
        {
            lookTarget = null;
        }
    }
    void LateUpdate()
    {
        if (lookTarget == null || hoverTextRoot == null) return;

        // always face camera smoothly
        hoverTextRoot.transform.rotation = Quaternion.LookRotation(hoverTextRoot.transform.position - lookTarget.position);
    }
}
