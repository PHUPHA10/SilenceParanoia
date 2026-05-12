using UnityEngine;

public class TriggerTaxi : MonoBehaviour, IInteractable
{

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "";

    [Header("Prompt Text")]
    [SerializeField] private string prompt = "เพื่อขึ้นรถแท็กซี่";
    public string Prompt => prompt;

    public void Interact()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("Taxiscene");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }
}
