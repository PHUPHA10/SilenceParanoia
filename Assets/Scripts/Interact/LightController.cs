using UnityEngine;

public class LightController : MonoBehaviour, IInteractable
{
    [Header("Lights")]
    public Light[] lightsToEnable;

    [Header("Prompt")]
    [SerializeField] private string prompt = "เปิดไฟ";

    public string Prompt => prompt;

    private bool isOn = false;

    public void Interact()
    {
        isOn = !isOn;

        foreach (Light lightObj in lightsToEnable)
        {
            if (lightObj != null)
            {
                lightObj.enabled = isOn;
            }
        }
    }
}