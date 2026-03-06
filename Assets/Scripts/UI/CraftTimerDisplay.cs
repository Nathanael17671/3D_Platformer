using UnityEngine;
using UnityEngine.UI;

public class CraftTimerDisplay : MonoBehaviour
{
    public CraftingTable table;
    public Image fillImage;
    public GameObject progressBar;

    void Update()
    {
        if (table == null) return;
        if (table.GetProgress() > 0)
        {
            progressBar.SetActive(true);
            fillImage.fillAmount = table.GetProgress();
        } else progressBar.SetActive(false);
    }
}