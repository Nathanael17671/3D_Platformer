using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingTable : MonoBehaviour
{
    [Header("References")]
    public RecipeDatabase recipeDatabase;
    public PotionDatabase potionDatabase;

    public PlayerInteractController playerController;
    public Camera playerCamera;
    public GameObject numberPadCanvas;

    public Transform interactTarget;
    public Transform spawnPoint;

    [Header("Crafting Mode")]
    public CraftMode craftMode;

    [Header("Generator")]
    public PotionType generatedPotion;

    [Header("Restrictions")]
    [SerializeField] private bool restrictIngredients = false;
    public List<PotionType> allowedIngredients = new List<PotionType>();

    [Header("Number Input")]
    public NumberInputDevice numberInput;

    [Header("Slots")]
    public List<PotionSlot> slots = new List<PotionSlot>();

    [Header("Crafting")]
    public float craftDuration = 5f;
    public bool requireFullSlots = true;

    List<PotionType> storedIngredients = new List<PotionType>();

    public List<PotionType> GetStoredIngredients()
    {
        return new List<PotionType>(storedIngredients);
    }

    bool crafting;
    float craftTimer;

    void Start()
    {
        foreach (var slot in slots)
        {
            if (slot.slotObject == null) continue;

            Renderer r = slot.slotObject.GetComponent<Renderer>();

            if (r != null)
                slot.originalMaterial = r.material;
        }
    }

    void Update()
    {
        if (crafting)
            return;

        if (Input.GetMouseButtonDown(0) && IsLookingAtTarget())
        {
            if (CanStartCraft())
            {
                if (craftMode == CraftMode.CodeRecipe)
                    return;

                if (!AreAllSlotsFull())
                    return;

                StartCoroutine(CraftRoutine());
            }
        }
        HandleInsert();
    }

    
    bool CanStartCraft()
    {
        if (craftMode == CraftMode.Generate)
            return true;

        if (requireFullSlots && storedIngredients.Count < slots.Count)
            return false;

        return storedIngredients.Count > 0;
    }

    IEnumerator CraftRoutine()
    {
        crafting = true;
        craftTimer = 0;

        while (craftTimer < craftDuration)
        {
            craftTimer += Time.deltaTime;
            yield return null;
        }

        FinishCraft();

        crafting = false;
    }

    void FinishCraft()
    {
        switch (craftMode)
        {
            case CraftMode.Mix:
                CraftRecipe();
                break;

            case CraftMode.CodeRecipe:
                CraftRecipeWithCode();
                break;

            case CraftMode.Generate:
                SpawnPotion(generatedPotion);
                break;

            case CraftMode.Undo:
                UndoRecipe();
                break;

            case CraftMode.RestrictedIngredients:
                CraftRestricted();
                break;
        }

        storedIngredients.Clear();
        UpdateSlotVisuals();
    }

    void CraftRecipe()
    {
        PotionType result = recipeDatabase.TryCraft(storedIngredients, 0);

        if (result == PotionType.None)
            SpawnIngredients(storedIngredients);
        else
            SpawnPotion(result);
    }

    public void StartCraftingWithCode(int code)
    {
        if (craftMode != CraftMode.CodeRecipe)
        {
            Debug.LogWarning("Trying to start code craft on a non-CodeRecipe table!");
            return;
        }

        if (!AreAllSlotsFull())
        {
            Debug.Log("Cannot craft: slots not full");
            return;
        }

        if (crafting) return;

        // Assign number to input
        if (numberInput != null)
            numberInput.currentCode = code;

        StartCoroutine(CraftRoutine());
    }

    void CraftRecipeWithCode()
    {
        if (numberInput == null)
        {
            Debug.LogWarning("CodeRecipe requires a NumberInputDevice!");
            SpawnIngredients(storedIngredients);
            return;
        }

        int code = numberInput.currentCode;

        // Only match recipes that require a number
        PotionType result = recipeDatabase.TryCraftWithNumber(storedIngredients, code);

        if (result == PotionType.None)
        {
            // Recipe failed → return ingredients
            SpawnIngredients(storedIngredients);
        }
        else
        {
            SpawnPotion(result);
        }
    }
    bool IsIngredientAllowed(PotionType type)
    {
        if (!restrictIngredients)
            return true;

        return allowedIngredients.Contains(type);
    }
    void CraftRestricted()
    {
        foreach (var i in storedIngredients)
        {
            if (!allowedIngredients.Contains(i))
            {
                SpawnIngredients(storedIngredients);
                return;
            }
        }

        CraftRecipe();
    }

    void UndoRecipe()
    {
        PotionType potion = storedIngredients[0];

        List<PotionType> ingredients = recipeDatabase.FindIngredientsForResult(potion);

        if (ingredients != null)
            SpawnIngredients(ingredients);
        else
            SpawnPotion(potion);
    }

    bool AreAllSlotsFull()
    {
        if (storedIngredients == null)
            return false;

        return storedIngredients.Count >= slots.Count;
    }

    void HandleInsert()
    {
        

        if (!IsLookingAtTarget())
            return;
        
        if (crafting)
            return;

        if (playerController.HeldObject == null)
        {
            if (Input.GetMouseButtonDown(1))
                RemoveLastIngredient();
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        PotionBehavior potion = playerController.HeldObject.GetComponent<PotionBehavior>();

        if (potion == null)
            return;

        if (storedIngredients.Count >= slots.Count)
            return;
        
        if (!IsIngredientAllowed(potion.potionType))
            return;


        storedIngredients.Add(potion.potionType);

        Destroy(playerController.HeldObject.gameObject);

        UpdateSlotVisuals();
    }

    void RemoveLastIngredient()
    {
        if (storedIngredients.Count == 0)
            return;

        PotionType type = storedIngredients[storedIngredients.Count - 1];
        storedIngredients.RemoveAt(storedIngredients.Count - 1);

        SpawnPotion(type);

        UpdateSlotVisuals();
    }

    void SpawnPotion(PotionType type)
    {
        GameObject prefab = potionDatabase.GetPrefab(type);

        if (prefab != null)
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    void SpawnIngredients(List<PotionType> ingredients)
    {
        foreach (var i in ingredients)
        {
            GameObject prefab = potionDatabase.GetPrefab(i);

            if (prefab != null)
                Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void UpdateSlotVisuals()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Renderer r = slots[i].slotObject.GetComponent<Renderer>();

            if (r == null)
                continue;

            if (i < storedIngredients.Count)
            {
                PotionEntry entry = potionDatabase.GetEntry(storedIngredients[i]);

                if (entry != null && entry.slotMaterial != null)
                    r.material = entry.slotMaterial;
            }
            else
            {
                r.material = slots[i].originalMaterial;
            }
        }
    }

    bool IsLookingAtTarget()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, 5f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == interactTarget)
                return true;
        }

        return false;
    }

    public float GetProgress()
    {
        if (!crafting) return 0;

        return craftTimer / craftDuration;
    }
}