using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CraftingTableBase : MonoBehaviour
{
    [Header("References")]
    public RecipeDatabase recipeDatabase;
    public PotionDatabase potionDatabase;

    public PlayerInteractController playerController;
    public Camera playerCamera;

    public Transform interactTarget;
    public Transform spawnPoint;

    [Header("Ingredient Display Slots")]
    [SerializeField] protected List<PotionSlot> slots = new List<PotionSlot>();

    [Header("Optional Number Input")]
    public NumberInputDevice numberInput;

    [Header("Crafting")]
    public float craftDuration = 5f;

    protected List<PotionType> storedIngredients = new List<PotionType>();

    bool crafting;
    float craftTimer;

    protected virtual void Start()
    {
        foreach (var slot in slots)
        {
            if (slot.slotObject != null)
            {
                Renderer r = slot.slotObject.GetComponent<Renderer>();

                if (r != null)
                    slot.originalMaterial = r.material;
            }
        }
    }

    public float GetProgress()
    {
        if (!crafting) return 0;
        return craftTimer / craftDuration;
    }

    protected void StartCrafting()
    {
        if (!crafting)
            StartCoroutine(CraftingRoutine());
    }

    IEnumerator CraftingRoutine()
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

    protected abstract void FinishCraft();

    protected void SpawnPotion(PotionType type)
    {
        GameObject prefab = potionDatabase.GetPrefab(type);

        if (prefab != null)
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    protected void SpawnIngredients(List<PotionType> ingredients)
    {
        foreach (var i in ingredients)
        {
            GameObject prefab = potionDatabase.GetPrefab(i);

            if (prefab != null)
                Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    protected bool IsLookingAtTarget(float distance = 5f)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == interactTarget)
                return true;
        }

        return false;
    }

    protected void UpdateSlotVisuals()
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
}
