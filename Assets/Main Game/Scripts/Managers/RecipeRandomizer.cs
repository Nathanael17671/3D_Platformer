using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

[System.Serializable]
public class AbstractRecipe
{
    public List<AbstractPotion> ingredients;
    public bool usesNumber;
    public AbstractPotion result;

    [HideInInspector] public int generatedCode; // ✅ unique per recipe
}

[System.Serializable]
public class FixedMapping
{
    public AbstractPotion abstractPotion;
    public PotionType fixedPotion;
}

public class RecipeRandomizer : MonoBehaviour
{
    [SerializeField] private RecipeDatabase recipeDatabase;
    [SerializeField] private PotionDatabase potionDatabase;
    [SerializeField] private List<FixedMapping> fixedMappings;

    [Header("Abstract Recipes")]
    [SerializeField] private List<AbstractRecipe> abstractRecipes;

    [Header("All Possible Potions")]
    [SerializeField] private List<PotionType> availablePotions;

    [Header("UI")]
    [SerializeField] private List<TMP_Text> recipeTexts;

    private Dictionary<AbstractPotion, PotionType> mapping = new Dictionary<AbstractPotion, PotionType>();

    private int generatedCode;

    void Awake()
    {
        recipeTexts = recipeTexts.OrderBy(x => Random.value).ToList(); 
        GenerateMapping();
        GenerateCodes();
        BuildRealRecipes();
        recipeDatabase.BuildDatabase();
        DisplayRecipes();
    }

    void GenerateMapping()
    {
        mapping.Clear();

        // Start with full pool
        List<PotionType> pool = new List<PotionType>(availablePotions);

        // 1️⃣ Apply fixed mappings first
        foreach (var fixedMap in fixedMappings)
        {
            mapping[fixedMap.abstractPotion] = fixedMap.fixedPotion;
            pool.Remove(fixedMap.fixedPotion);
        }

        // 2️⃣ Fill remaining abstract potions randomly
        foreach (AbstractPotion abs in System.Enum.GetValues(typeof(AbstractPotion)))
        {
            if (mapping.ContainsKey(abs))
                continue;

            if (pool.Count == 0)
                break;

            int index = Random.Range(0, pool.Count);

            mapping[abs] = pool[index];
            pool.RemoveAt(index);
        }
    }

    void GenerateCodes()
    {
        HashSet<int> usedCodes = new HashSet<int>();

        foreach (var recipe in abstractRecipes)
        {
            if (!recipe.usesNumber)
                continue;

            int code;

            do
            {
                code = Random.Range(1000, 9999);
            }
            while (usedCodes.Contains(code));

            usedCodes.Add(code);
            recipe.generatedCode = code;
        }
    }

    void BuildRealRecipes()
    {
        foreach (var recipe in abstractRecipes)
        {
            List<PotionType> realIngredients = new List<PotionType>();

            foreach (var abs in recipe.ingredients)
            {
                if (!mapping.ContainsKey(abs))
                {
                    Debug.LogError("Missing mapping for: " + abs);
                    return;
                }

                realIngredients.Add(mapping[abs]);
            }

            if (!mapping.ContainsKey(recipe.result))
            {
                Debug.LogError("Missing result mapping for: " + recipe.result);
                return;
            }

            PotionType result = mapping[recipe.result];

            recipeDatabase.AddRecipe(
                realIngredients,
                recipe.usesNumber,
                recipe.generatedCode,
                result
            );
        }
    }

    void DisplayRecipes()
    {
        for (int i = 0; i < abstractRecipes.Count && i < recipeTexts.Count; i++)
        {
            var recipe = abstractRecipes[i];

            string text = "";

            // Ingredients
            for (int j = 0; j < recipe.ingredients.Count; j++)
            {
                PotionType real = mapping[recipe.ingredients[j]];
                text += real.ToString();

                if (j < recipe.ingredients.Count - 1)
                {
                    text += " + ";
                }
                else if (!recipe.usesNumber && recipe.ingredients.Count < 2)
                {
                    text += " + Furnace";
                }
                    
            }

            // Number
            if (recipe.usesNumber)
            {
                text += " + " + recipe.generatedCode;
            }

            // Result
            PotionType result = mapping[recipe.result];

            text += " = " + result.ToString();

            recipeTexts[i].text = text;
        }
    }
    
    // Access for crafting system
    public PotionType GetMapped(AbstractPotion abs)
    {
        return mapping[abs];
    }

    public int GetCode()
    {
        return generatedCode;
    }

    public GameObject GetPrefab(AbstractPotion abs)
    {
        if (!mapping.ContainsKey(abs))
        {
            Debug.LogError("No mapping for " + abs);
            return null;
        }

        PotionType real = mapping[abs];
        return potionDatabase.GetPrefab(real);
    }
}