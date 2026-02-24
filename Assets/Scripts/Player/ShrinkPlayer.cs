using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed = 1f;
    [SerializeField] private float startHeight = 200f;
    [SerializeField] private float minHeight = 20f;
    [SerializeField] private bool active = true;
    private float currentheight;

    void Start()
    {
        currentheight = startHeight;
    }
    
    void Update()
    {
        if(active == true)
        {
           if(currentheight > minHeight)
            {
                currentheight -=  currentheight * 0.01f * shrinkSpeed * Time.deltaTime;
                Debug.Log(currentheight);
            }
            else
            {
                active = false;
                Debug.Log("You have hit the minimum height");
            } 
        }
        
        
    }
}
