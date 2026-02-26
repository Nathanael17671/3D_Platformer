using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed = 1f;
    [SerializeField] private float startScale = 200f;
    [SerializeField] private float minScale = 20f;
    [SerializeField] private bool active = true;
    private float currentScale;
    private Vector3 setScale;

    void Start()
    {
        currentScale = startScale;
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
            }
            else
            {
                active = false;
                Debug.Log("You have hit the minimum height");
            } 
        }
        
        
    }
}
