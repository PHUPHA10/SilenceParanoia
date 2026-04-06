using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject loadButton;

    void Start()
    {
        //แค่ควบคุมปุ่ม ห้ามโหลดเกมตรงนี้
        bool hasSave = PlayerPrefs.GetInt("HasSave", 0) == 1;
        bool completed = PlayerPrefs.GetInt("GameCompleted", 0) == 1;

        if (!hasSave || completed)
        {
            if (loadButton != null)
                loadButton.SetActive(false);
        }
        else
        {
            if (loadButton != null)
                loadButton.SetActive(true);
        }

        //รีเซ็ต cursor กันบัค
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;
    }

    public void NewGame()
    {
        SaveSystem.DeleteSave();

        PlayerPrefs.SetInt("GameCompleted", 0);
        PlayerPrefs.SetInt("HasSave", 1);

        AutoSaveManager.GameStarted = true;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("Taxiscene");
        }
    }

    public void LoadGame()
    {
        bool hasSave = PlayerPrefs.GetInt("HasSave", 0) == 1;
        bool completed = PlayerPrefs.GetInt("GameCompleted", 0) == 1;

        if (!hasSave || completed)
            return;

        SaveData data = SaveSystem.LoadGame();

        if (data == null || string.IsNullOrEmpty(data.sceneName))
            return;

        AutoSaveManager.GameStarted = true;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene(data.sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}