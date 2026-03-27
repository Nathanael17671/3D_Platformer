using UnityEngine;
using UnityEngine.UI; // Required for uGUI

public class ToggleHandler : MonoBehaviour
{
    public Toggle toggle;
    public GameObject sizeBar;

    void Awake()
    {
        toggle.isOn = DataManager.Instance.showHeight;
        ShowHeighToggle(toggle.isOn);
        // Add a listener to the toggle's onValueChanged event
        toggle.onValueChanged.AddListener(delegate {
            ShowHeighToggle(toggle.isOn);
        });

        // Set initial state
        
    }

    // This public method can be called by the On Value Changed event or directly by script
    public void ShowHeighToggle(bool isShowing)
    {
        DataManager.Instance.showHeight = isShowing;
        if (sizeBar != null)
            sizeBar.SetActive(isShowing);
    }
}