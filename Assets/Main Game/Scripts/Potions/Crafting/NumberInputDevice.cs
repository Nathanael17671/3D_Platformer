using UnityEngine;

public class NumberInputDevice : MonoBehaviour
{
    public int currentCode;
    [SerializeField] private LayerMask interactionLayers;

    [Header("References")]
    public NumberPad numberPad;
    public CraftingTable craftingTable;
    [SerializeField] private PlayerInteractController playerInteractController;
    public Camera playerCamera;
    [SerializeField] private ProximityScript proximityScript;

    public void SetCode(int code)
    {
        currentCode = code;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, playerInteractController.pickupDistance, interactionLayers))
            {
                if (hit.transform == transform)
                {
                    if (numberPad != null)
                    {
                        if (craftingTable.requireFullSlots && craftingTable.storedIngredients.Count < craftingTable.slots.Count)
                        {
                            proximityScript.DisplayRoutine();
                        } 
                        else
                        {
                            numberPad.Enable(this, craftingTable);
                        }
                    }
                }
            }
        }
    }
}