using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [HideInInspector] public bool isPaused = false;

    [Header("Referances")]
    [SerializeField] private GameObject[] activateOnStart;
    [SerializeField] private MonoBehaviour[] scriptsToDisable;
    [SerializeField] private AudioSource[] audioSourcesToMute;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ShrinkPlayer shrinkPlayer;
    [SerializeField] private NumberPad numberPad;
    [SerializeField] private SoundManager soundManager;

    [Header("Victory")] 
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private AudioClip victorySound;
    [HideInInspector] public bool victoryTriggered = false;

    [Header("Defeat")]
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private AudioClip defeatSound;
    [HideInInspector] public bool defeatTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var gameObject in activateOnStart)
            gameObject.SetActive(true);
        UnPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (numberPad.isActive == false)
            {
                if (isPaused == true)
                {
                    UnPause();
                    soundManager.PlaySmallButtonUpSFX();
                }
                else
                {
                    Pause();
                    soundManager.PlaySmallButtonDownSFX();
                }
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
        settingsMenu.SetActive(false);
        EnableControlls();
        isPaused = false;
        UnMuteAll();
    } 

    public void Pause()
    {
        pauseMenu.SetActive(true);
        DisableControlls();
        isPaused = true;
        MuteAll();
    }

    public void MuteAll()
    {
        foreach (AudioSource source in audioSourcesToMute)
        {
            if (source != null)
            {
                source.mute = true;
            }
        }
    }

    // Optional: A function to unmute specifically
    public void UnMuteAll()
    {
        foreach (AudioSource source in audioSourcesToMute)
        {
            if (source != null)
            {
                source.mute = false;
            }
        }
    }
}
