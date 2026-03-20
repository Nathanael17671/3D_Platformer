using System.Collections.Generic;
using UnityEngine;

public class RecipeDatabase : MonoBehaviour
{
    [SerializeField] private List<PotionRecipe> recipes = new List<PotionRecipe>();

    private Dictionary<string, PotionType> recipeLookup = new Dictionary<string, PotionType>();

    public void BuildDatabase()
    {
        recipeLookup.Clear();

        foreach (var recipe in recipes)
        {
            string key = RecipeKeyGenerator.Generate(
                recipe.ingredients,
                recipe.useNumber,
                recipe.requiredCode
);
            recipeLookup[key] = recipe.result;
        }
    }

    public PotionType TryCraft(List<PotionType> ingredients, int code)
    {
        string keyWithNumber = RecipeKeyGenerator.Generate(ingredients, true, code);

        if (recipeLookup.TryGetValue(keyWithNumber, out PotionType result))
            return result;

        string keyWithoutNumber = RecipeKeyGenerator.Generate(ingredients, false, 0);

        if (recipeLookup.TryGetValue(keyWithoutNumber, out result))
            return result;

        return PotionType.None;
    }

    // Only matches recipes that require a number
    public PotionType TryCraftWithNumber(List<PotionType> ingredients, int code)
    {
        string keyWithNumber = RecipeKeyGenerator.Generate(ingredients, true, code);

        if (recipeLookup.TryGetValue(keyWithNumber, out PotionType result))
            return result;

        // Do NOT fall back to ignoring numbers
        return PotionType.None;
    }

    public List<PotionType> FindIngredientsForResult(PotionType result)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.result == result)
                return new List<PotionType>(recipe.ingredients);
        }

        return null;
    }

    public void AddRecipe(List<PotionType> ingredients, bool useNumber, int code, PotionType result)
    {
        // 1️⃣ Add to dictionary (used for crafting)
        string key = RecipeKeyGenerator.Generate(ingredients, useNumber, code);
        recipeLookup[key] = result;

        // 2️⃣ Add to list (used for lookup/debugging)
        PotionRecipe newRecipe = new PotionRecipe
        {
            ingredients = new List<PotionType>(ingredients),
            useNumber = useNumber,
            requiredCode = code,
            result = result
        };

        recipes.Add(newRecipe);

        Debug.Log("Added recipe: " + key + " → " + result);
    }
}