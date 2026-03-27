using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeInn : MonoBehaviour
{
    public Image imageRenderer;
    private float currentFade;
    [SerializeField] private float fadeOverTime = 2; 
    public bool FadeOut = false;

    void Start()
    {
        currentFade = 1;
        if (FadeOut)
            currentFade = 0;
        if (imageRenderer == null)
        {
            imageRenderer = GetComponent<Image>();
        }
    }

    void Update()
    {
        if (FadeOut)
        {
            if (currentFade < 1)
                currentFade += Time.deltaTime / fadeOverTime;
            SetImageOpacity(currentFade);
            }
            else
            {
            if (currentFade > 0)
                currentFade -= Time.deltaTime / fadeOverTime;
            else
                this.gameObject.SetActive(false);
            SetImageOpacity(currentFade);
        }
    }

    public void SetImageOpacity(float alphaValue)
    {
        Color tempColor = imageRenderer.color;

        tempColor.a = alphaValue;

        imageRenderer.color = tempColor;
    }
}