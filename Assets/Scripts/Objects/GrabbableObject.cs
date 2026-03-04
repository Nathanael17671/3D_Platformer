using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Transform grabPoint;

    [Header("Weight")]
    public float weight = 10f;

    [Header("Follow Settings")]
    [SerializeField] private float followForce = 3000;
    [SerializeField] private float damping = 1500;
    [SerializeField] private float rotationForce = 0.5f;

    [Header("Distance")]
    [SerializeField] private float maxHoldDistance = 7f;

    [Header("Heavy Object Behavior")]
    [SerializeField] private float maxLiftHeight = 0.5f;
    [SerializeField] private float dragWhenTooHeavy = 8f;

    [Header("Hover Text")]
    [SerializeField] private GameObject hoverTextRoot;
    [SerializeField] private TextMeshProUGUI hoverText;

    [SerializeField] private string objectName = "Object";


    private Transform lookTarget;
    public float objectPlayerStrength;
    private Vector3 lastGrabPointPos;
    private Vector3 grabPointVelocity;

    public bool IsHeld => grabPoint != null;




    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }



    // ================= GRAB =================
    public void Grab(Transform grabTransform, float strength)
    {
        grabPoint = grabTransform;
        objectPlayerStrength = strength;

        rb.useGravity = true;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        lastGrabPointPos = grabPoint.position;
    }



    // ================= DROP =================
    public void Drop()
    {
        grabPoint = null;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.angularVelocity *= 0.2f;
        float weightFactor = Mathf.Clamp01(objectPlayerStrength / weight);
        rb.linearVelocity = grabPointVelocity * 0.6f * weightFactor;

    }

    public bool TooHeavyToLift(float strength)
    {
        return weight > strength * 3f;
    }

    public float WeightRatio(float strength)
    {
        return Mathf.Clamp01(weight / strength);
    }



    // ================= PHYSICS =================
    void FixedUpdate()
    {
        if (grabPoint == null) return;

        grabPointVelocity = (grabPoint.position - lastGrabPointPos) / Time.fixedDeltaTime;
        lastGrabPointPos = grabPoint.position;

        Vector3 target = grabPoint.position;
        Vector3 direction = target - transform.position;

        float distance = direction.magnitude;
        if (distance > maxHoldDistance || TooHeavyToLift(objectPlayerStrength))
        {
            Drop();
            return;
        }

        float liftCapability = objectPlayerStrength / weight;
        Vector3 adjustedTarget = target;


        // ===== HEAVY OBJECT HANDLING =====
        if (liftCapability < 1f)
        {
            // too heavy → can't lift fully
            float maxY = transform.position.y + maxLiftHeight * liftCapability;
            adjustedTarget.y = Mathf.Min(adjustedTarget.y, maxY);

            // simulate ground friction drag
            rb.linearDamping = dragWhenTooHeavy * (1f - liftCapability);
            rb.angularDamping = dragWhenTooHeavy * (1f - liftCapability) * 0.5f;
        }
        else
        {
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }

        direction = adjustedTarget - transform.position;

        // Non-linear follow force for heavier objects
        float forceMultiplier = Mathf.Pow(liftCapability, 0.6f); 
        Vector3 force = direction * (followForce * forceMultiplier) - rb.linearVelocity * damping;

        rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Acceleration);


        // ===== ROTATION SPRING =====
        Quaternion targetRotation = grabPoint.rotation;

        // force the object to have 0 rotation on the world X-axis
        Vector3 worldEuler = targetRotation.eulerAngles;
        worldEuler.x = 0f;
        targetRotation = Quaternion.Euler(worldEuler);

        // compute torque
        Quaternion delta = targetRotation * Quaternion.Inverse(transform.rotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;

        Vector3 torque = axis * angle * rotationForce - rb.angularVelocity * 5f;
        rb.AddTorque(torque, ForceMode.Acceleration);
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
