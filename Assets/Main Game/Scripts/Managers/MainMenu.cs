using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Level_Tutorial");
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level_2");
    }
    public void LoadDevLvl()
    {
        SceneManager.LoadScene("Level_0");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is Exiting");
    }
        
}
