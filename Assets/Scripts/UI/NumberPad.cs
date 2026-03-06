using UnityEngine;
using TMPro;

public class NumberPad : MonoBehaviour
{
    [Header("Display")]
    public TextMeshProUGUI displayText;

    [Header("Connected Devices")]
    private NumberInputDevice connectedDevice;
    private CraftingTable connectedTable;

    private bool isActive = true;
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    private string currentInput = "";

    void Start()
    {
        Disable();
    }

    public void Enable(NumberInputDevice device, CraftingTable table)
    {
        if (isActive == true) return;
        isActive = true;

        connectedDevice = device;
        connectedTable = table;

        currentInput = "";
        UpdateDisplay();

        foreach (var script in scriptsToDisable)
        script.enabled = false;

        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (isActive == false) return;
        isActive = false;

        foreach (var script in scriptsToDisable)
        script.enabled = true;

        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;

        currentInput = "";
        UpdateDisplay();

        gameObject.SetActive(false);
    }

    public void AddDigit(int digit)
    {
        if (currentInput.Length >= 4)
            return;

        currentInput += digit.ToString();
        UpdateDisplay();

        if (currentInput.Length == 4)
        {
            int code = int.Parse(currentInput);

            if (connectedDevice != null)
                connectedDevice.SetCode(code);

            if (connectedTable != null)
                connectedTable.StartCraftingWithCode(code);

            Disable();
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (displayText != null)
        {
            if (currentInput == "")
                displayText.text = "----";
            else
                displayText.text = currentInput.PadRight(4, '-');
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Disable();
    }
}