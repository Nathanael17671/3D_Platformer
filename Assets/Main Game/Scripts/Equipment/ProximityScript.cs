using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ProximityScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private GameObject proximityDisplay;
    [SerializeField] private bool proximityOn = true;

    [Header("Hover Text")]
    [SerializeField] private GameObject hoverTextRoot;
    [SerializeField] private TextMeshProUGUI hoverText;
    [SerializeField] private string objectName = "Object";


    private float delay;
    private Transform lookTarget;

    void Start()
    {
        proximityDisplay.SetActive(false);
        SetHover(false, playerCameraTransform);
    }

    void Update()
    {
        if (!proximityOn)
            Routine();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!proximityOn)
            return;
        if (other.CompareTag("Player"))
        {
            proximityDisplay.SetActive(true);
            SetHover(true, playerCameraTransform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!proximityOn)
            return;
        if (other.CompareTag("Player"))
        {
            proximityDisplay.SetActive(false);
            SetHover(false, playerCameraTransform);
        }
    }

    public void DisplayRoutine()
    {
        delay = 3f;
    }

    void Routine()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            proximityDisplay.SetActive(true);
            SetHover(true, playerCameraTransform);
        }
        else
        {
            proximityDisplay.SetActive(false);
            SetHover(false, playerCameraTransform);
        }
    }
        



    public void SetHover(bool state, Transform cameraTransform)
    {
        
        if (hoverTextRoot == null) return;

        hoverTextRoot.SetActive(state);

        if (state)
        {
            lookTarget = cameraTransform;

            
            hoverText.text = objectName;
    
        }
        else
        {
            lookTarget = null;
        }
    }
    void LateUpdate()
    {
        if (lookTarget == null || hoverTextRoot == null) return;

        // always face camera smoothly
        hoverTextRoot.transform.rotation = Quaternion.LookRotation(hoverTextRoot.transform.position - lookTarget.position);
    }
}
