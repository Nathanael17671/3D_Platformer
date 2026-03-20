using UnityEngine;
using System.Collections.Generic;

public class ScatterObjects : MonoBehaviour
{
    public List<GameObject> objectsToPlace = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        ScatterObject();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) ScatterObject();
    }

    void ScatterObject()
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