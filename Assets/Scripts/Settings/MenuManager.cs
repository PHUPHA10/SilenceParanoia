using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("LastScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}