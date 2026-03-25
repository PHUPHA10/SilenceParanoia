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
    public float spinSpeed = 200f;

    void Awake()
    {
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

        loadingPanel.SetActive(false);
    }

    void Update()
    {

        if (loadingPanel.activeSelf && spinner != null)
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

        loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }


        yield return null;


        loadingPanel.SetActive(false);
    }
}