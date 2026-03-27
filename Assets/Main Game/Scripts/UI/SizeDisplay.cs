using UnityEngine;
using UnityEngine.UI;

public class SizeDisplay : MonoBehaviour
{
    public ShrinkPlayer shrinkPlayer;
    public Image fillImage;
    public GameObject progressBar;

    void Update()
    {
        if (shrinkPlayer == null) return;
            fillImage.fillAmount = shrinkPlayer.progressBar;
    }
}