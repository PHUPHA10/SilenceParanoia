using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsRoot;
    public GameObject Continue;

    [Header("Scene To Load")]
    public string loadGameScene;

    [Header("UI Settings")]
    public CanvasGroup uiCanvasGroup; 
    public float fadeDuration = 2.0f;

    [Header("Canvas Setup")]
    public GameObject mainMenuCanvas; 
    public GameObject gameplayCanvas;

    [Header("Timeline Setup")]
    public PlayableDirector startCutscene;
    void Start()
    {
        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 0f;
            StartCoroutine(FadeInUI());
        }

        mainMenu.SetActive(true);
        settingsRoot.SetActive(false);
        Continue.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsRoot.SetActive(true);
        Continue.SetActive(false);
    }

    public void BackToMenu()
    {
        settingsRoot.SetActive(false);
        mainMenu.SetActive(true);
        Continue.SetActive(false);
        if (startCutscene != null)
        {
            StartCoroutine(PlayTimelineBackwards());
        }
    }

    public void LoadGame()
    {
        if (SceneLoader.Instance != null)
       {
            SceneLoader.Instance.LoadScene(loadGameScene);
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void SwitchToGameplay()
    {
        // ปิด Canvas หน้าเมนู
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }

        // เปิด Canvas หน้าเล่นเกม
        if (gameplayCanvas != null)
        {
            gameplayCanvas.SetActive(true);
        }
    }
    public void StartNewGame()
    {
        // 1. สั่งให้ Timeline เริ่มเล่น (เช่น กล้องเลื่อน)
        if (startCutscene != null)
        {
            startCutscene.gameObject.SetActive(true);
            startCutscene.Play();
        }

        // 2. ปิด Canvas หน้าเมนู
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }

        // 3. เปิด Canvas หน้าเล่นเกม
        if (gameplayCanvas != null)
        {
            gameplayCanvas.SetActive(true);
        }
    }
    IEnumerator PlayTimelineBackwards()
    {
        // สั่งหยุดการเล่นแบบปกติก่อน
        startCutscene.Pause();

        // ดึงเวลาปัจจุบันของ Timeline ออกมา (ถ้าเล่นจบแล้ว มันจะเป็นค่าเวลาสูงสุด)
        float currentTime = (float)startCutscene.time;

        // ถ้าเวลาเป็น 0 อยู่แล้ว (แปลว่ายังไม่ได้เล่น) ให้ตั้งค่าเป็นเวลาตอนจบ
        if (currentTime <= 0) currentTime = (float)startCutscene.duration;

        // ลูปเพื่อลดเวลาลงเรื่อยๆ (เหมือนการกรอวิดีโอถอยหลัง)
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // หักลบเวลาตามเฟรมเรท
            startCutscene.time = currentTime; // ยัดเวลาใหม่กลับเข้าไปใน Timeline
            startCutscene.Evaluate(); // บังคับให้ Timeline อัปเดตภาพตามเวลาที่เปลี่ยนไป

            yield return null; // รอให้จบเฟรมนี้ แล้วค่อยทำต่อเฟรมหน้า
        }

        // เมื่อถอยหลังจนสุดแล้ว บังคับให้เวลาเป็น 0 เป๊ะๆ เพื่อความชัวร์
        startCutscene.time = 0;
        startCutscene.Evaluate();
    }
    IEnumerator FadeInUI()
    {
        float elapsedTime = 0f;

        // ลูปนี้จะทำงานไปเรื่อยๆ จนกว่าเวลาจะครบกำหนด
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // ค่อยๆ ปรับค่า Alpha จาก 0 ไป 1 ตามเวลาที่ผ่านไป
            uiCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            yield return null; // รอเฟรมถัดไป
        }

        // ยืนยันให้ค่าเป็น 1 เสมอเมื่อจบการทำงาน
        uiCanvasGroup.alpha = 1f;

        // หากต้องการให้ UI กดคลิกได้หลังจากโผล่มาแล้ว
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;
    }
}