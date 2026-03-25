using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI")]
    public GameObject loadingPanel;
    public RectTransform spinner;

    [Header("Settings")]
    public float spinSpeed = 180f;

    void Awake()
    {
        // 🔥 Singleton กันซ้ำ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (loadingPanel == null)
        {
            loadingPanel = GameObject.Find("LoadingPanel");
        }

        if (spinner == null && loadingPanel != null)
        {
            spinner = loadingPanel.GetComponentInChildren<RectTransform>();
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    void Update()
    {

        if (loadingPanel != null && loadingPanel.activeSelf && spinner != null)
        {
            spinner.Rotate(Vector3.forward * -spinSpeed * Time.deltaTime);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return null;

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}