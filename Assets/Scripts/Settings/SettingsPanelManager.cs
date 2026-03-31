using UnityEngine;

public class SettingsPanelManager : MonoBehaviour
{
    public GameObject audioPanel;
    public GameObject graphicsPanel;
    public GameObject controlsPanel;

    void OnEnable()
    {
        ShowAudio();
    }

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        graphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void ShowGraphics()
    {
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
}