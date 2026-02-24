using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject toDoList;
    [SerializeField] private bool show = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (show == true)
            {
                toDoList.SetActive(true);

            } else
            {
                toDoList.SetActive(false);
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (show == true)
            {
                show = false;
                toDoList.SetActive(false);

            } else
            {
                show = true;
                toDoList.SetActive(true);
            }
            
        }
    }
}
