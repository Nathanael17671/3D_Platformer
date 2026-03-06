using UnityEngine;
using System.Collections.Generic;

public class RandomObjectSpawn : MonoBehaviour
{
    public List<GameObject> objectsToPlace = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        ScatterObjects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) ScatterObjects();
    }

    void ScatterObjects()
    {
        List<Transform> availableSpots = new List<Transform>(spawnPoints);

        foreach (GameObject obj in objectsToPlace)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            int index = Random.Range(0, availableSpots.Count);

            rb.position = availableSpots[index].position;
            rb.rotation = availableSpots[index].rotation;

            availableSpots.RemoveAt(index); // prevents reuse
            rb = null;
        }
        Debug.Log("Scatter");
    }
}