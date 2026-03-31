using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoSaveManager : MonoBehaviour
{
    static AutoSaveManager instance;

    public static bool GameStarted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!GameStarted)
            return;

        StartCoroutine(WaitAndSave());
    }

    IEnumerator WaitAndSave()
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            AutoSave(player.transform);
        }
        else
        {
            Debug.LogWarning("Player not found for AutoSave");
        }
    }

    void AutoSave(Transform player)
    {
        SaveSystem.SaveGame(player.position, SceneManager.GetActiveScene().name);
        Debug.Log("Auto Saved");
    }
}