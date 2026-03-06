using UnityEngine;

public class NumberInputDevice : MonoBehaviour
{
    public int currentCode;

    [Header("References")]
    public NumberPad numberPad;
    public CraftingTable craftingTable;

    public void SetCode(int code)
    {
        currentCode = code;
    }

    void OnMouseDown()
    {
        if (numberPad != null)
        {
            numberPad.Enable(this, craftingTable);
        }
    }
}