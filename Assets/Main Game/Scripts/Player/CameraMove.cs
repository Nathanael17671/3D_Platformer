using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    //How much the camera moves based on mouse movement
    [Header("Laptop = 30")]
    [Header("PC = 80")]

    //currentX is public since we use it to orient the player
    private float weightMultiplier = 1f;
    public float currentX;
    private float currentY;

    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    //Set the Minimum and Maximim y position to limit the camera from going to far up or down
    private const float YMin = -90.0f;
    private const float YMax = 90.0f;

    void Start()
    {
        playerTransform.transform.localRotation = Quaternion.Euler(0, currentX, 0);
    }
    // LateUpdate is called once per frame after Update is run
    void LateUpdate()
    {
        //Get the input of the mouse and add it to the current position of the camera (-1 to invert the camera inputs)
        currentX += Input.GetAxis("Mouse X") * DataManager.Instance.sensitivity * weightMultiplier * 10f * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * DataManager.Instance.sensitivity * weightMultiplier * 14f * Time.deltaTime * -1;

        //Limit y camera movement
        currentY = Mathf.Clamp(currentY, YMin, YMax);
        //Set the rotation of the camera to the current values
        transform.rotation = Quaternion.Euler(currentY, currentX, 0);
    }

    public void SetWeightMultiplier(float value)
    {
        weightMultiplier = value;
    }
}
