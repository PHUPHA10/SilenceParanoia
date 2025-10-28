using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game0");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SettingGame()
    {
        SceneManager.LoadScene("Option");
    }
}
