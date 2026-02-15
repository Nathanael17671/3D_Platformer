using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private const float YMin = -90.0f;
    private const float YMax = 90.0f;

    public Transform lookAt;

    public Transform Player;

    public float distance = 0.1f;
    public float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensivity = 4.0f;
    
    // Update is called once per frame
    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensivity * 10 * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * 10 * Time.deltaTime * -1;

        currentY = Mathf.Clamp(currentY, YMin, YMax);
        Debug.Log(currentX + currentY);
        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = rotation;

        transform.LookAt(lookAt.position);
    }

    
}
