using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject toDoList;
    [SerializeField] private bool show = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToDoList(false);
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToDoList(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ToDoList(bool tdl)
    {
        if (show == tdl)
        {
            show = false;
            toDoList.SetActive(false);
        } 
        else
        {
            show = true;
            toDoList.SetActive(true);
        }
    }
}
