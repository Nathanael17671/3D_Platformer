using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject toDoList;
    [SerializeField] private bool showTDL = false;

    [Header("Referances")]
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private AudioSource audioSource;

    [Header("Victory")]
    public bool victoryTriggered = false;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private AudioClip victorySound;

    [Header("Defeat")]
    public bool defeatTriggered = false;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private AudioClip defeatSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToDoList(false);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void TriggerVictory()
    {
        victoryTriggered = true;

        DisableControlls();
        Debug.Log("Victory!");

        victoryScreen.SetActive(true);
        audioSource.PlayOneShot(victorySound);
        // You can replace this later with:
        // UI win screen
        // cutscene
        // level transition
        // etc.
    }

    public void TriggerDefeat()
    {
        defeatTriggered = true;

        DisableControlls();
        Debug.Log("Defeat...");

        defeatScreen.SetActive(true);
        audioSource.PlayOneShot(defeatSound);
        // You can replace this later with:
        // UI win screen
        // cutscene
        // level transition
        // etc.
    }

    public void DisableControlls()
    {
        foreach (var script in scriptsToDisable)
        script.enabled = false;

        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;
    }

    public void EnableControlls()
    {
        foreach (var script in scriptsToDisable)
        script.enabled = true;

        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        defeatScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ToDoList(bool tdl)
    {
        if (showTDL == tdl)
        {
            showTDL = false;
            toDoList.SetActive(false);
        } 
        else
        {
            showTDL = true;
            toDoList.SetActive(true);
        }
    }
}
