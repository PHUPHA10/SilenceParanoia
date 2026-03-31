using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        AutoSaveManager.GameStarted = true;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("Taxiscene");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data != null)
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadScene(data.sceneName);
            }
            else
            {
                Debug.LogWarning("SceneLoader not found!");
            }
        }
        else
        {
            Debug.LogWarning("No save data found!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}