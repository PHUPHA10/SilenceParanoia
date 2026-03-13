using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Taxiscene");
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}