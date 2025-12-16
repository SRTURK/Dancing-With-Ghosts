using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void LoadEasyMode()
    {
        SceneManager.LoadScene("EasyMode");
    }

    public void LoadHardMode()
    {
        SceneManager.LoadScene("HardMode");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
