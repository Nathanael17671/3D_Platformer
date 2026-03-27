using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private bool xActive = true;
    [SerializeField] private float rotationSpeedX = 5f;
    [SerializeField] private bool yActive = true;
    [SerializeField] private float rotationSpeedY = 5f;
    [SerializeField] private bool zActive = true;
    [SerializeField] private float rotationSpeedZ = 5f;

    [Header("On/Off")]
    public bool active = false;
    private float originalXRotation;
    private float originalYRotation;
    private float originalZRotation;

    private float currentXRotation;
    private float currentYRotation;
    private float currentZRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalXRotation = this.transform.eulerAngles.x;
        originalYRotation = this.transform.eulerAngles.y;
        originalZRotation = this.transform.eulerAngles.z;

        currentXRotation = originalXRotation;
        currentYRotation = originalYRotation;
        currentZRotation = originalZRotation;
    }

    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        this.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, currentZRotation);

        if (!active)
        {
            currentXRotation = originalXRotation;
            currentYRotation = originalYRotation;
            currentZRotation = originalZRotation;
            return;
        }

        if (xActive)
        {
            currentXRotation += rotationSpeedX / 10f;
            if (currentXRotation > 360f)
                currentXRotation -= 360f;
            if (currentXRotation < -360f)
                currentXRotation += 360f;
        } else currentXRotation = originalXRotation;
        if (yActive)
        {
            currentYRotation += rotationSpeedY / 10f;
            if (currentYRotation > 360f)
                currentYRotation -= 360f;
            if (currentYRotation < -360f)
                currentYRotation += 360f;
        } else currentYRotation = originalYRotation;
        if (zActive)
        {
            currentZRotation += rotationSpeedZ / 10f;
            if (currentZRotation > 360f)
                currentZRotation -= 360f;
            if (currentZRotation < -360f)
                currentZRotation += 360f;
        } else currentZRotation = originalZRotation;
    }

}
