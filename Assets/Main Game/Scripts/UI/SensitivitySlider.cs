using UnityEngine;
using UnityEngine.UI; // Required for UI Slider
using TMPro;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider uiSlider;
    [SerializeField] private TextMeshProUGUI sliderText;

    void Start()
    {
        // Initialize UI with inspector value
        uiSlider.value = DataManager.Instance.sensitivity;
        sliderText.text = uiSlider.value.ToString("0");
        // Listen for UI changes to update inspector value
        uiSlider.onValueChanged.AddListener(OnSliderChanged);
        
    }

    void OnSliderChanged(float value)
    {
        DataManager.Instance.sensitivity = value;
        sliderText.text = value.ToString("0");
    }
}
