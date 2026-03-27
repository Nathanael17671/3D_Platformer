using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] private Slider uiSlider;
    [SerializeField] private TextMeshProUGUI sliderText;

    public void DropdownSample(int index)
    {
        switch (index)
        {
            case 0: 
                DisableSliderInput();
                uiSlider.value = 3f;
                DataManager.Instance.shrinkPlayerSpeed = uiSlider.value;
                sliderText.text = uiSlider.value.ToString("0");
                Debug.Log(uiSlider.value);
                break;

            case 1: 
                DisableSliderInput();
                uiSlider.value = 5f;
                DataManager.Instance.shrinkPlayerSpeed = uiSlider.value;
                sliderText.text = uiSlider.value.ToString("0");
                Debug.Log(uiSlider.value);
                break;

            case 2: 
                DisableSliderInput();
                uiSlider.value = 10f;
                DataManager.Instance.shrinkPlayerSpeed = uiSlider.value;
                sliderText.text = uiSlider.value.ToString("0");
                Debug.Log(uiSlider.value);
                break;
            
            case 3: 
                EnableSliderInput();
                Debug.Log(uiSlider.value);
                break;
        }
    }

    

    void Start()
    {
        // Initialize UI with inspector value
        uiSlider.value = DataManager.Instance.shrinkPlayerSpeed;
        sliderText.text = uiSlider.value.ToString("0");
        // Listen for UI changes to update inspector value
        uiSlider.onValueChanged.AddListener(OnSliderChanged);
        
    }

    void OnSliderChanged(float value)
    {
        DataManager.Instance.shrinkPlayerSpeed = value;
        sliderText.text = value.ToString("0.0");
    }

    public void DisableSliderInput()
    {
        if (uiSlider != null)
        {
            uiSlider.interactable = false;
        }
    }

    public void EnableSliderInput()
    {
        if (uiSlider != null)
        {
            uiSlider.interactable = true;
        }
    }
}
