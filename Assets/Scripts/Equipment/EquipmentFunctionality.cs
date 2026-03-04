using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PotionEntry
{
    public string potionName;
    public GameObject prefab;
    public Material slotMaterial; // material to show when this potion is stored
}

[System.Serializable]
public class PotionSlot
{
    public GameObject slotObject; // object representing this slot in world
    [HideInInspector] public Material originalMaterial;
}

public class EquipmentFunctionality : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInteractController playerController;
    [SerializeField] private Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] private Transform interactTarget; // must look at this to store/use
    [SerializeField] private float interactDistance = 5f;

    [Header("Potions")]
    [SerializeField] private List<PotionEntry> potionPrefabs = new List<PotionEntry>();
    [SerializeField] private List<PotionSlot> potionSlots = new List<PotionSlot>();
    [SerializeField] private int maxPotions = 3;
    [SerializeField] private Transform spawnPoint;

    private List<string> storedPotions = new List<string>();

    void Start()
    {
        // save original materials
        foreach (var slot in potionSlots)
        {
            if (slot.slotObject != null)
            {
                Renderer r = slot.slotObject.GetComponent<Renderer>();
                if (r != null)
                    slot.originalMaterial = r.material;
            }
        }
    }

    void LateUpdate()
    {
        HandlePotionPickup();
        HandlePotionUse();
    }

    void HandlePotionPickup()
    {
        if (Input.GetMouseButtonDown(0)) // left click = store potion
        {
            if (playerController.HeldObject != null)
            {
                PotionBehavior potion = playerController.HeldObject.GetComponent<PotionBehavior>();
                if (potion != null)
                {
                    // Only allow if looking at interactTarget
                    if (!IsLookingAtTarget()) return;

                    if (storedPotions.Count >= maxPotions)
                    {
                        Debug.Log("Cannot pick up more potions. Max reached: " + maxPotions);
                        return;
                    }

                    storedPotions.Add(potion.potionName);
                    UpdateSlotVisuals();

                    Destroy(playerController.HeldObject.gameObject);

                    Debug.Log("Stored potion: " + potion.potionName + " | Total stored: " + storedPotions.Count);
                }
            }
        }
    }

void HandlePotionUse()
{
    // Do not allow taking potions out while holding something
    if (playerController.IsHoldingObject)
        return;

    if (!Input.GetMouseButtonDown(1))
        return;

    if (storedPotions.Count == 0)
        return;

    if (!IsLookingAtTarget())
        return;

    int lastIndex = storedPotions.Count - 1;
    string potionName = storedPotions[lastIndex];
    storedPotions.RemoveAt(lastIndex);

    UpdateSlotVisuals();

    GameObject prefabToSpawn = GetPrefabByName(potionName);
    if (prefabToSpawn != null && spawnPoint != null)
    {
        Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
    }
}

    bool IsLookingAtTarget()
{
    if (interactTarget == null || playerCamera == null) return false;

    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

    RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);

    foreach (RaycastHit hit in hits)
    {
        // Ignore held object and ALL its children
        if (playerController.HeldObject != null)
        {
            Transform heldRoot = playerController.HeldObject.transform;

            if (hit.transform == heldRoot || hit.transform.IsChildOf(heldRoot))
                continue; // skip this hit and keep checking
        }

        // Found interact target
        if (hit.transform == interactTarget)
            return true;
    }

    return false;
}

    GameObject GetPrefabByName(string name)
    {
        foreach (var entry in potionPrefabs)
        {
            if (entry.potionName == name)
                return entry.prefab;
        }
        return null;
    }

    PotionEntry GetPotionEntryByName(string name)
    {
        foreach (var entry in potionPrefabs)
        {
            if (entry.potionName == name)
                return entry;
        }
        return null;
    }

    void UpdateSlotVisuals()
{
    for (int i = 0; i < potionSlots.Count; i++)
    {
        Renderer r = potionSlots[i].slotObject.GetComponent<Renderer>();
        if (r == null) continue;

        // If slot has a stored potion
        if (i < storedPotions.Count)
        {
            PotionEntry entry = GetPotionEntryByName(storedPotions[i]);

            if (entry != null && entry.slotMaterial != null)
                r.material = entry.slotMaterial;
        }
        else
        {
            // Empty slot → restore original material
            r.material = potionSlots[i].originalMaterial;
        }
    }
}

    public List<string> GetStoredPotions()
    {
        return new List<string>(storedPotions);
    }
}