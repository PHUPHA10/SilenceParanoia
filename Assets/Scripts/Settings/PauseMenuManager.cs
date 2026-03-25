using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsRoot;
    public GameObject chatCanvas;
    public MonoBehaviour playerController;
    public MonoBehaviour mouseLook;

    [Header("Canvases To Disable When Paused")]
    public List<GameObject> canvasesToDisable;
    public AudioSource gameAudio;

    public static bool IsPaused = false;
    public static bool IsVideoPlaying = false;

    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        if (IsVideoPlaying) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PhoneChat.IsChatOpen)
            {
                PhoneChat phone = FindObjectOfType<PhoneChat>();
                if (phone != null)
                    phone.CloseChat();
                return;
            }

            if (!IsPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        foreach (GameObject canvas in canvasesToDisable)
            canvas.SetActive(false);

        if (gameAudio != null)
            gameAudio.enabled = false;

        IsPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        if (playerController != null) playerController.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;

        UnlockCursor();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingsRoot.SetActive(false);

        foreach (GameObject canvas in canvasesToDisable)
        {
            if (canvas != chatCanvas)
                canvas.SetActive(true);
        }

        if (gameAudio != null)
            gameAudio.enabled = true;

        IsPaused = false;
        Time.timeScale = 1f;

        if (playerController != null) playerController.enabled = true;
        if (mouseLook != null) mouseLook.enabled = true;

        // 🔒 ล็อคเมาส์กลับ
        LockCursor();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenSettings()
    {
        settingsRoot.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        settingsRoot.SetActive(false);
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        IsPaused = true;

        if (playerController != null) playerController.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;

        UnlockCursor();
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }
}