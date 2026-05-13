using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DrinkSequenceManager : MonoBehaviour
{
    public Image fadeImage;
    public VideoPlayer videoPlayer;

    public float animationDuration = 10f; // ความยาว animation ดื่มน้ำ

    [Header("Objects To Disable During Video")]
    public List<GameObject> objectsToDisable;
    public AudioSource gameAudio;
    public float fadeInSpeed = 1.2f;

    [Header("Next Scene")]
    public string nextSceneName = "MainMenu";

    public void StartDrinkSequence()
    {
        StartCoroutine(DrinkSequence());
    }

    IEnumerator DrinkSequence()
    {
        if (gameAudio != null)
            gameAudio.enabled = false;

        // ล็อก ESC ตั้งแต่เริ่ม sequence
        PauseMenuManager.IsVideoPlaying = true;

        // รอ animation ดื่มน้ำ
        yield return new WaitForSeconds(animationDuration);

        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeInSpeed;

            fadeImage.color = new Color(
                0f,
                0f,
                0f,
                alpha
            );

            yield return null;
        }
        yield return new WaitForSeconds(3f);

        fadeImage.gameObject.SetActive(false);

        // ปิด object ต่าง ๆ
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 🔥 เล่นวิดีโอหลังจอดำแล้ว
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();

        // รอให้วิดีโอเริ่มเล่นก่อน
        yield return new WaitUntil(() => videoPlayer.isPlaying);

        // รอจนวิดีโอจบ
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        PauseMenuManager.IsVideoPlaying = false;

        // เปลี่ยนซีน
        SceneManager.LoadScene(nextSceneName);
    }
}