using UnityEngine;

public class QTEGuideController : MonoBehaviour
{
    public GameObject guidePanel;

    public GameObject page1;
    public GameObject page2;
    public GameObject page3;

    [Header("Canvases To Disable")]
    public GameObject[] canvasesToDisable;


    private void Start()
    {

    }

    public void ShowGuide()
    {
        guidePanel.SetActive(true);


        MicrophoneInput.StopMic();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisableCanvases();
        ShowPage1();
    }

    void DisableCanvases()
    {
        foreach (var canvas in canvasesToDisable)
        {
            if (canvas != null)
                canvas.SetActive(false);
        }
    }

    public void ShowPage1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
    }

    public void ShowPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
        page3.SetActive(false);
    }

    public void ShowPage3()
    {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(true);
    }
    void EnableCanvases()
    {
        foreach (var canvas in canvasesToDisable)
        {
            if (canvas != null)
                canvas.SetActive(true);
        }
    }

    public void CloseGuide()
    {
        guidePanel.SetActive(false);
        Time.timeScale = 1f;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EnableCanvases();
        MicrophoneInput.StartMic();
    }
}