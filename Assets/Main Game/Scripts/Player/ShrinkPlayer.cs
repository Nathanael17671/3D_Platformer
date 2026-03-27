using System;
using Unity.VisualScripting;
using UnityEngine;

using TMPro;

public class ShrinkPlayer : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed = 5f;
    [SerializeField] private float startScale = 200f;
    [SerializeField] private float minScale = 20f;
    [SerializeField] public bool active = true;
    private PlayerInteractController playerInteractController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private GameObject objectGrabPoint;
    private float currentPickupDistance;
    private float currentGrabPoint;
    private float currentGrabStrengh;
    public float currentScale;
    private Vector3 setScale;

    [Header("Display size")]
    [SerializeField] TextMeshProUGUI maximumSizeText;
    [SerializeField] TextMeshProUGUI minimumSizeText;
    [SerializeField] TextMeshProUGUI currentSizeText;
    [HideInInspector] public float progressBar;
    [HideInInspector] public float currentSizeTextPosition;


    void Start()
    {
        if (maximumSizeText != null)
        {
            maximumSizeText.text = startScale.ToString("0" + "cm");
            minimumSizeText.text = minScale.ToString("0" + "cm");
        }
        
        shrinkSpeed = DataManager.Instance.shrinkPlayerSpeed;
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
                if (maximumSizeText != null)
                {
                currentSizeText.text = currentScale.ToString("0" + "cm");
                progressBar = Mathf.InverseLerp(minScale, startScale, currentScale);
                currentSizeTextPosition = Mathf.Lerp(-450, 450, progressBar);
                currentSizeText.transform.localPosition = new Vector3(currentSizeText.transform.localPosition.x, currentSizeTextPosition,0);
                }

                currentScale -=  currentScale * 0.001f * shrinkSpeed * Time.deltaTime;
                //Debug.Log(currentScale);
                setScale = new Vector3(currentScale,currentScale,currentScale) / 200f;
                transform.localScale = setScale;

                currentPickupDistance -=  currentPickupDistance * 0.0005f * shrinkSpeed * Time.deltaTime;
                playerInteractController.pickupDistance = currentPickupDistance;

                currentGrabStrengh -=  currentGrabStrengh * 0.001f * shrinkSpeed * Time.deltaTime;
                playerInteractController.playerStrength = currentGrabStrengh;
                //Debug.Log(currentGrabStrengh);

                currentGrabPoint +=  currentGrabPoint * 0.0005f * shrinkSpeed * Time.deltaTime;
                objectGrabPoint.transform.localPosition = new Vector3(objectGrabPoint.transform.localPosition.x, objectGrabPoint.transform.localPosition.y, currentGrabPoint);
           }
            else
            {
                active = false;
                Debug.Log("You have hit the minimum height");
                gameManager.TriggerDefeat();
            } 
        }
        
    }
}
