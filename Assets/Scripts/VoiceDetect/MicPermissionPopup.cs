using UnityEngine;
using System.Collections;

public class MicPermissionPopup : MonoBehaviour
{
    public GameObject popupPanel;

    [Header("Images")]
    public GameObject normalImage;
    public GameObject warningImage;

    [Header("Delay")]
    public float delayTime = 1f;

    void Start()
    {
        //ถ้าเคยเลือกแล้ว → ไม่ต้องโชว์ popup
        if (PlayerPrefs.HasKey("MicAllowed"))
        {
            popupPanel.SetActive(false);
            return;
        }

        popupPanel.SetActive(true);
        Time.timeScale = 0f;

        if (normalImage != null) normalImage.SetActive(false);
        if (warningImage != null) warningImage.SetActive(false);

        StartCoroutine(ShowNormalAfterDelay());
    }

    IEnumerator ShowNormalAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayTime);

        if (normalImage != null)
            normalImage.SetActive(true);
    }

    public void AllowMic()
    {
        //ไม่มีไมค์ → แสดงเตือน
        if (Microphone.devices.Length == 0)
        {
            if (normalImage != null) normalImage.SetActive(false);
            if (warningImage != null) warningImage.SetActive(true);
            return;
        }

        // บันทึกว่าอนุญาติ
        MicPermissionManager.AllowMic = true;
        PlayerPrefs.SetInt("MicAllowed", 1);
        PlayerPrefs.Save();

        ClosePopup();
    }

    public void DenyMic()
    {
        //บันทึกว่าไม่อนุญาติ
        MicPermissionManager.AllowMic = false;
        PlayerPrefs.SetInt("MicAllowed", 0);
        PlayerPrefs.Save();

        ClosePopup();
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}