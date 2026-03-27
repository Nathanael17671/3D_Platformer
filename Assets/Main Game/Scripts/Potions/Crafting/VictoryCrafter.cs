using UnityEngine;
using System.Collections.Generic;

public class VictoryCrafter : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private CraftingTable craftingTable;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject endDialouge;
    [SerializeField] private ShrinkPlayer shrinkPlayer;

    [Header("Allowed Ingredients")]
    [SerializeField] private List<PotionType> requiredIngredients = new List<PotionType>();

    private bool end = false;

    void Update()
    {
        if (gameManager.victoryTriggered)
            return;

        List<PotionType> ingredients = craftingTable.GetStoredIngredients();

        if (ingredients.Count != requiredIngredients.Count)
            return;

        // check if ingredients match required set
        foreach (PotionType type in requiredIngredients)
        {
            if (!ingredients.Contains(type))
                return;
        }
        if (end == false)
        {
            end = true;
            shrinkPlayer.active = false;
            endDialouge.SetActive(true);
        }
        
    }

    
}