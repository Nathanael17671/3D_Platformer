using UnityEngine;

public class IngredientCraftingTable : CraftingTableBase
{
    [Header("Craft Rules")]
    [SerializeField] bool requireFullSlots = true;
    public int maxIngredients = 3;

    void Update()
    {
        HandleInsert();
        HandleCraft();
    }

    void HandleInsert()
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

        if (storedIngredients.Count >= maxIngredients)
            return;

        storedIngredients.Add(potion.potionType);

        UpdateSlotVisuals();

        Destroy(playerController.HeldObject.gameObject);
    }

    void HandleCraft()
{
    if (!Input.GetMouseButtonDown(1))
        return;

    if (!IsLookingAtTarget())
        return;

//    if (storedIngredients.Count == 0)
  //      return;

    if (requireFullSlots && storedIngredients.Count < maxIngredients)
    {
        Debug.Log("Not all slots filled.");
        return;
    }

    StartCrafting();
}

    protected override void FinishCraft()
    {
        PotionType result = recipeDatabase.TryCraft(
            storedIngredients,
            numberInput != null ? numberInput.currentCode : 0
        );

        if (result == PotionType.None)
        {
            Debug.Log("Invalid recipe → returning ingredients");

            SpawnIngredients(storedIngredients);
        }
        else
        {
            SpawnPotion(result);
        }

        storedIngredients.Clear();
        UpdateSlotVisuals();
    }
}