using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Move : MonoBehaviour
{
    [SerializeField] private bool local = false;

    [Header("X")]
    [SerializeField] private bool xActive = true;
    [SerializeField] private float upperXLimit = 5f;
    [SerializeField] private float lowerXLimit = 5f;
    [SerializeField] private float moveSpeedX = 5f;

    [Header("Y")]
    [SerializeField] private bool yActive = true;
    [SerializeField] private float upperYLimit = 5f;
    [SerializeField] private float lowerYLimit = 5f;
    [SerializeField] private float moveSpeedY = 5f;

    [Header("Z")]
    [SerializeField] private bool zActive = true;
    [SerializeField] private float upperZLimit = 5f;
    [SerializeField] private float lowerZLimit = 5f;
    [SerializeField] private float moveSpeedZ = 5f;
    
    
    [Header("On/Off")]
    public bool active = false;

    private float originalXPosition;
    private float originalYPosition;
    private float originalZPosition;

    private float currentXPosition;
    private float currentYPosition;
    private float currentZPosition;

    private bool flipSwitchX;
    private bool flipSwitchY;
    private bool flipSwitchZ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (local)
        {
            originalXPosition = this.transform.localPosition.x;
            originalYPosition = this.transform.localPosition.y;
            originalZPosition = this.transform.localPosition.z;
        }
        else
        {
            originalXPosition = this.transform.position.x;
            originalYPosition = this.transform.position.y;
            originalZPosition = this.transform.position.z;
        }
        

        currentXPosition = originalXPosition;
        currentYPosition = originalYPosition;
        currentZPosition = originalZPosition;
    }

    // Update is called once per frame
    void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        if (local)
            this.transform.localPosition = new Vector3(currentXPosition, currentYPosition, currentZPosition);
        else
            this.transform.position = new Vector3(currentXPosition, currentYPosition, currentZPosition);

        if (!active)
        {
            currentXPosition = originalXPosition;
            currentYPosition = originalYPosition;
            currentZPosition = originalZPosition;
            return;
        }

        if (xActive)
        {
            if (flipSwitchX)
                currentXPosition += moveSpeedX / 500f;
            else
                currentXPosition -= moveSpeedX / 500f;
        } else currentXPosition = originalXPosition;
        if (yActive)
        {
            if (flipSwitchY)
                currentYPosition += moveSpeedY / 500f;
            else
                currentYPosition -= moveSpeedY / 500f;
        } else currentYPosition = originalYPosition;
        if (zActive)
        {
            if (flipSwitchZ)
                currentZPosition += moveSpeedZ / 500f;
            else
                currentZPosition -= moveSpeedZ / 500f;
        } else currentZPosition = originalZPosition;

        if (currentXPosition > originalXPosition + upperXLimit/5f)
            flipSwitchX = false;
        else if (currentXPosition < originalXPosition - lowerXLimit/5f)
            flipSwitchX = true;
        if (currentYPosition > originalYPosition + upperYLimit/5f)
            flipSwitchY = false;
        else if (currentYPosition < originalYPosition - lowerYLimit/5f)
            flipSwitchY = true;
        if (currentZPosition > originalZPosition + upperZLimit/5f)
            flipSwitchZ = false;
        else if (currentZPosition < originalZPosition - lowerZLimit/5f)
            flipSwitchZ = true;
    }
}
