using UnityEngine;

public class SceneInteractTrigger : MonoBehaviour, IInteractable
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "";

    [Header("Prompt Text")]
    [SerializeField] private string prompt = "เข้าลิฟต์เพื่อไปที่ห้อง";
    public string Prompt => prompt;

    public void Interact()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("Lastscene");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }
}