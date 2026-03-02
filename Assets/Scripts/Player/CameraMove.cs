using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //How much the camera moves based on mouse movement
    [Header("Laptop = 20, 20")]
    [Header("PC = 50, 50")]
    [SerializeField][Range(10.0f, 120.0f)]private float sensivityX = 50.0f;
    [SerializeField][Range(10.0f, 120.0f)]private float sensivityY = 50.0f;

    //currentX is public since we use it to orient the player
    private float weightMultiplier = 1f;
    [HideInInspector] public float currentX;
    private float currentY;

    //Set the Minimum and Maximim y position to limit the camera from going to far up or down
    private const float YMin = -90.0f;
    private const float YMax = 90.0f;
    
    // LateUpdate is called once per frame after Update is run
    void LateUpdate()
    {
        //Get the input of the mouse and add it to the current position of the camera (-1 to invert the camera inputs)
        currentX += Input.GetAxis("Mouse X") * sensivityX * weightMultiplier * 10 * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivityY * weightMultiplier * 10 * Time.deltaTime * -1;

        //Limit y camera movement
        currentY = Mathf.Clamp(currentY, YMin, YMax);
        //Set the rotation of the camera to the current values
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = rotation;
    }

    public void SetWeightMultiplier(float value)
    {
        weightMultiplier = value;
    }
}
