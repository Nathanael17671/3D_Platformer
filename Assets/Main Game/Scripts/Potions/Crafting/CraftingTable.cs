using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingTable : MonoBehaviour
{
    [Header("References")]
    [Header("Managers")]
    [SerializeField] private RecipeDatabase recipeDatabase;
    [SerializeField] private PotionDatabase potionDatabase;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;

    [Header("Player")]
    [SerializeField] private PlayerInteractController playerController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask interactMask;

    [Header("Misc")]
    [SerializeField] private Transform interactTarget;
    [SerializeField] private Transform spawnPoint;

    [Header("Visuals")]
    [SerializeField] private GameObject displayOnCraft = null;
    [SerializeField] private Rotate[] rotationScript = null;
    [SerializeField] private Move[] moveScript = null;

    [Header("Audio")]
    [SerializeField] private AudioSource craftingSFX;
    [SerializeField] private AudioSource finishCraftingSFX;

    [Header("Crafting Mode")]
    public CraftMode craftMode;

    [Header("Generator")]
    public PotionType generatedPotion;

    [Header("Restrictions")]
    [SerializeField] private bool restrictIngredients = false;
    public List<PotionType> allowedIngredients = new List<PotionType>();

    [Header("Number Input")]
    [SerializeField] private GameObject numberPadCanvas;
    public NumberInputDevice numberInput;

    [Header("Slots")]
    public List<PotionSlot> slots = new List<PotionSlot>();

    [Header("Crafting")]
    public float craftDuration = 5f;
    public bool requireFullSlots = true;

    [HideInInspector] public List<PotionType> storedIngredients = new List<PotionType>();

    

    public List<PotionType> GetStoredIngredients()
    {
        return new List<PotionType>(storedIngredients);
    }
    [HideInInspector] public bool finishFirstCraft = false;
    [HideInInspector] public bool crafting;
    float craftTimer;

    void Start()
    {
        if (displayOnCraft != null)
            displayOnCraft.SetActive(false);
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

        if (Input.GetMouseButtonDown(0) && IsLookingAtTarget() && gameManager.isPaused == false)
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
        if (displayOnCraft != null)
            displayOnCraft.SetActive(true);
        if (rotationScript != null)
            foreach (var script in rotationScript)
                script.active = true;
        if (moveScript != null)
            foreach (var script in moveScript)
                script.active = true;
        
        crafting = true;
        craftTimer = 0;
        craftingSFX.Play();

        while (craftTimer < craftDuration)
        {
            if (gameManager.isPaused == false)
            {
                craftTimer += Time.deltaTime;
            }
            yield return null;
        }

        FinishCraft();

        if (displayOnCraft != null)
            displayOnCraft.SetActive(false);
        if (rotationScript != null)
            foreach (var script in rotationScript)
                script.active = false;
        if (moveScript != null)
            foreach (var script in moveScript)
                script.active = false;
        
        crafting = false;
        craftingSFX.Stop();
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
        
        finishCraftingSFX.Play();
        storedIngredients.Clear();
        UpdateSlotVisuals();
    }

    void CraftRecipe()
    {
        PotionType result = recipeDatabase.TryCraft(storedIngredients, 0);

        if (result == PotionType.None)
            SpawnIngredients(storedIngredients);
        else
        {
            finishFirstCraft = true;
            SpawnPotion(result);
        }
            
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
            finishFirstCraft = true;
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
        soundManager.PlayPotionInsertSFX();
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
        soundManager.PlayRecipeFailSFX();
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
        if (interactTarget == null || playerCamera == null)
            return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playerController.pickupDistance, interactMask))
        {
            return hit.transform == interactTarget;
        }

        return false;
    }

    public float GetProgress()
    {
        if (!crafting) return 0;

        return craftTimer / craftDuration;
    }
}