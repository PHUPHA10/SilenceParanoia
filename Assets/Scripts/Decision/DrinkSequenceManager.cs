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
    public float fadeSpeed = 2f;

    [Header("Objects To Disable During Video")]
    public List<GameObject> objectsToDisable;

    [Header("Next Scene")]
    public string nextSceneName = "MainMenu"; // ใส่ชื่อซีนที่ต้องการโหลด

    public void StartDrinkSequence()
    {
        StartCoroutine(DrinkSequence());
    }

    IEnumerator DrinkSequence()
    {
        // ล็อก ESC ตั้งแต่เริ่ม sequence
        PauseMenuManager.IsVideoPlaying = true;

        yield return new WaitForSeconds(animationDuration);

        Color c = fadeImage.color;

        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();

        // รอให้วิดีโอเริ่มเล่นก่อน
        yield return new WaitUntil(() => videoPlayer.isPlaying);

        // รอจนวิดีโอจบ
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        PauseMenuManager.IsVideoPlaying = false;

        // เปลี่ยนซีนทันที
        SceneManager.LoadScene(nextSceneName);
    }
}