using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    public bool isPaused = false;

    [Header("Referances")]
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ShrinkPlayer shrinkPlayer;

    [Header("Victory")]
    [HideInInspector] public bool victoryTriggered = false;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private AudioClip victorySound;

    [Header("Defeat")]
    [HideInInspector] public bool defeatTriggered = false;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private AudioClip defeatSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == true)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
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
        shrinkPlayer.active = false;
    }

    public void EnableControlls()
    {
        foreach (var script in scriptsToDisable)
        script.enabled = true;

        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        shrinkPlayer.active = true;

        defeatScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        EnableControlls();
        isPaused = false;
    } 

    public void Pause()
    {
        pauseMenu.SetActive(true);
        DisableControlls();
        isPaused = true;
    }
}
