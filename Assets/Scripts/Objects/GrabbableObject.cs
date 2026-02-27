using UnityEngine;

public class GrabbableObject : MonoBehaviour
{

    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Vector3 previousPosition;
    private Vector3 newPosition;
    private float lerpspeed = 10f;



    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform){
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
    }

    public void Drop()
    {
        Vector3 throwVelocity = (newPosition - previousPosition) / Time.deltaTime / 1.5f;
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.linearVelocity = throwVelocity;

    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            previousPosition = newPosition;
            
            newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpspeed);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, objectGrabPointTransform.rotation, Time.deltaTime * 3f);
            objectRigidbody.MovePosition(newPosition);
            objectRigidbody.MoveRotation(newRotation);
        }
    }
}
