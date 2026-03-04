using UnityEngine;
//using System.Collections.Generic;

public class RandomObjectSpawn : MonoBehaviour
{
    public GameObject[] randomSpawnObjects;






/*/
    public List<GameObject> objectsToPlace;
    public List<Transform> spawnPoints;

    void Start()
    {
        ScatterObjects();
    }

    void ScatterObjects()
    {
        List<Transform> availableSpots = new List<Transform>(spawnPoints);

        foreach (GameObject obj in objectsToPlace)
        {
            int index = Random.Range(0, availableSpots.Count);

            obj.transform.position = availableSpots[index].position;

            availableSpots.RemoveAt(index); // prevents reuse
        }
    }
/*/
}