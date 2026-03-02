using System;
using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed = 1f;
    [SerializeField] private float startScale = 200f;
    [SerializeField] private float minScale = 20f;
    [SerializeField] private bool active = true;
    private PlayerInteractController playerInteractController;
    [SerializeField] private GameObject objectGrabPoint;
    private float currentPickupDistance;
    private float currentGrabPoint;
    private float currentGrabStrengh;
    private float currentScale;
    private Vector3 setScale;


    void Start()
    {
        playerInteractController = GetComponent<PlayerInteractController>();
        currentScale = startScale;

        currentPickupDistance = playerInteractController.pickupDistance;

        currentGrabStrengh = playerInteractController.playerStrength;

        currentGrabPoint = objectGrabPoint.transform.localPosition.z;
        
    }
    
    void Update()
    {
        if(active == true)
        {
           if(currentScale > minScale)
            {
                currentScale -=  currentScale * 0.01f * shrinkSpeed * Time.deltaTime;
                Debug.Log(currentScale);
                setScale = new Vector3(currentScale,currentScale,currentScale) / 200f;
                transform.localScale = setScale;

                currentPickupDistance -=  currentPickupDistance * 0.0075f * shrinkSpeed * Time.deltaTime;
                playerInteractController.pickupDistance = currentPickupDistance;

                currentGrabStrengh -=  currentGrabStrengh * 0.025f * shrinkSpeed * Time.deltaTime;
                playerInteractController.playerStrength = currentGrabStrengh;
                Debug.Log(currentGrabStrengh);

                currentGrabPoint +=  currentGrabPoint * 0.005f * shrinkSpeed * Time.deltaTime;
                objectGrabPoint.transform.localPosition = new Vector3(objectGrabPoint.transform.localPosition.x, objectGrabPoint.transform.localPosition.y, currentGrabPoint);
           }
            else
            {
                active = false;
                Debug.Log("You have hit the minimum height");
            } 
        }
        
    }
}
