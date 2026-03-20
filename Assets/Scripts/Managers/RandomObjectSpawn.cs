using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecipeRandomizer recipeRandomizer;

    [Header("Spawn Setup")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Abstract Potions To Spawn")]
    [SerializeField] private List<AbstractPotion> potionsToSpawn = new List<AbstractPotion>();

    [Header("Settings")]
    [SerializeField] private bool randomRotation = true;

    void Start()
    {
        ScatterObjects();
    }

    void ScatterObjects()
    {
        if (recipeRandomizer == null)
        {
            Debug.LogError("RecipeRandomizer missing!");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        // Copy lists so we can modify them safely
        List<Transform> availableSpots = new List<Transform>(spawnPoints);
        List<AbstractPotion> spawnList = new List<AbstractPotion>(potionsToSpawn);

        // Shuffle spawn points
        for (int i = 0; i < availableSpots.Count; i++)
        {
            Transform temp = availableSpots[i];
            int randomIndex = Random.Range(i, availableSpots.Count);
            availableSpots[i] = availableSpots[randomIndex];
            availableSpots[randomIndex] = temp;
        }

        int spawnCount = Mathf.Min(spawnList.Count, availableSpots.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            AbstractPotion abs = spawnList[i];

            GameObject prefab = recipeRandomizer.GetPrefab(abs);

            if (prefab == null)
            {
                Debug.LogError("No prefab for " + abs);
                continue;
            }

            Transform point = availableSpots[i];

            Quaternion rot = randomRotation
                ? Quaternion.Euler(0, Random.Range(0, 360f), 0)
                : point.rotation;

            Instantiate(prefab, point.position, rot);
        }
    }
}