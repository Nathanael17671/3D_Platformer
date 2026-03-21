using UnityEngine;
using UnityEngine.UI; // Required for UI Slider
using TMPro;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider uiSlider;
    [SerializeField] private TextMeshProUGUI sliderText;

    void Start()
    {
        // Initialize UI with inspector value
        uiSlider.value = DataManager.Instance.musicVolume;
        sliderText.text = uiSlider.value.ToString("0");
        // Listen for UI changes to update inspector value
        uiSlider.onValueChanged.AddListener(OnSliderChanged);
        
    }

    void OnSliderChanged(float value)
    {
        DataManager.Instance.musicVolume = value;
        sliderText.text = value.ToString("0");
    }
}
