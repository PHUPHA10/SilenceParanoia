using UnityEngine;
using UnityEngine.UI;

public class BreathQTE : MonoBehaviour
{
    [Header("UI")]
    public Image redLungImage;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip coughSfx;

    [Header("Settings")]
    public float increaseSpeed = 0.4f;
    public float decreasePerPress = 0.15f;
    public float maxValue = 1f;
    public GameObject objectToHide;

    private float value;
    private bool active;

    void OnEnable()
    {
        StartQTE();

    }

    void Update()
    {
        if (!active) return;

        // ไม่กด = แดงขึ้น
        value += increaseSpeed * Time.deltaTime;

        // กด = ลด
        if (Input.GetKeyDown(KeyCode.Space))
        {
            value -= decreasePerPress;
        }

        value = Mathf.Clamp01(value);

        if (redLungImage != null)
            redLungImage.fillAmount = value;

        if (value > 0.8f)
        {
            value += increaseSpeed * 1.5f * Time.deltaTime;
        }


        // ❌ เต็ม = แพ้ทันที
        if (value >= maxValue)
        {
            PlayFailSfx();
            Fail();
            return;
        }
    }

    public void StartQTE()
    {
        active = true;
        value = 0f;

        if (redLungImage != null)
            redLungImage.fillAmount = 0f;
        if (objectToHide != null)
            objectToHide.SetActive(false);


    }

    void PlayFailSfx()
    {
        if (audioSource != null && coughSfx != null)
        {
            audioSource.PlayOneShot(coughSfx);
        }
    }

    void Fail()
    {
        active = false;

        if (HidingQTEManager.Instance != null &&
            HidingQTEManager.Instance.CurrentHideSpot != null)
        {
            HidingQTEManager.Instance.CurrentHideSpot.ForceExitHide();
        }

        gameObject.SetActive(false);
    }
}