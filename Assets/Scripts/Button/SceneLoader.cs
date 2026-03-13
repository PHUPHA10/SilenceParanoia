using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI")]
    public GameObject loadingPanel;
    public RectTransform spinner;

    [Header("Settings")]
    public float spinSpeed = 180f;

    bool isLoading = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    void Update()
    {
        if (isLoading && spinner != null)
        {
            spinner.Rotate(0f, 0f, -spinSpeed * Time.unscaledDeltaTime);
        }
    }

    public void ShowLoading()
    {
        isLoading = true;

        if (loadingPanel != null)
            loadingPanel.SetActive(true);
    }

    public void HideLoading()
    {
        isLoading = false;

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}