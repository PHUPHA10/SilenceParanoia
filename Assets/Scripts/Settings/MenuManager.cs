using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsRoot;

    [Header("Scene To Load")]
    public string loadGameScene;

    void Start()
    {
        mainMenu.SetActive(true);
        settingsRoot.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsRoot.SetActive(true);
    }

    public void BackToMenu()
    {
        settingsRoot.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadGame()
    {
        if (SceneLoader.Instance != null)
       {
            SceneLoader.Instance.LoadScene(loadGameScene);
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}