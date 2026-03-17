using UnityEngine;

public class NumberInputDevice : MonoBehaviour
{
    public int currentCode;
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private LayerMask interactionLayers;

    [Header("References")]
    public NumberPad numberPad;
    public CraftingTable craftingTable;

    public Camera playerCamera;

    public void SetCode(int code)
    {
        currentCode = code;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactionLayers))
            {
                if (hit.transform == transform)
                {
                    if (numberPad != null)
                    {
                        numberPad.Enable(this, craftingTable);
                    }
                }
            }
        }
    }
}