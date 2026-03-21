using UnityEngine;
using System.Collections.Generic;

public class UndoCraftingTable : CraftingTableBase
{
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (!IsLookingAtTarget())
            return;

        if (playerController.HeldObject == null)
            return;

        PotionBehavior potion = playerController.HeldObject.GetComponent<PotionBehavior>();

        if (potion == null)
            return;

        storedIngredients.Clear();
        storedIngredients.Add(potion.potionType);

        Destroy(playerController.HeldObject.gameObject);

        StartCrafting();
    }

    protected override void FinishCraft()
    {
        PotionType potion = storedIngredients[0];

        List<PotionType> ingredients = FindIngredientsForResult(potion);

        if (ingredients != null)
        {
            SpawnIngredients(ingredients);
        }
        else
        {
            SpawnPotion(potion); // cannot undo
        }

        storedIngredients.Clear();
    }

    List<PotionType> FindIngredientsForResult(PotionType result)
    {
        foreach (var recipe in recipeDatabase.GetType()
                 .GetField("recipes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                 .GetValue(recipeDatabase) as List<PotionRecipe>)
        {
            if (recipe.result == result)
                return recipe.ingredients;
        }

        return null;
    }
}