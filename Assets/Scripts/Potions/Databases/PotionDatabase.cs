using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionEntry
{
    public PotionType type;
    public GameObject prefab;
    public Material slotMaterial;
}

public class PotionDatabase : MonoBehaviour
{
    [SerializeField] private List<PotionEntry> potions = new List<PotionEntry>();

    public GameObject GetPrefab(PotionType type)
    {
        foreach (var p in potions)
            if (p.type == type)
                return p.prefab;

        return null;
    }

    public PotionEntry GetEntry(PotionType type)
    {
        foreach (var p in potions)
            if (p.type == type)
                return p;

        return null;
    }
}