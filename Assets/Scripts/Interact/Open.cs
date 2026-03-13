using UnityEngine;
using UnityEngine.Video;

public class Open : MonoBehaviour, IInteractable
{
    [Header("Item")]
    [SerializeField] private string itemName = "ทีวี";
    [SerializeField] private ItemDefinition itemData;

    [Header("TV")]
    public VideoPlayer videoPlayer;
    public AudioSource tvAudio;          // เสียงจากรายการทีวี

    [Header("TV SFX")]
    public AudioSource tvSfxAudio;       // 🔊 AudioSource สำหรับเสียงเปิด/ปิด
    public AudioClip tvOnSound;          // 🔊 เสียงเปิดทีวี
    public AudioClip tvOffSound;         // 🔊 เสียงปิดทีวี

    [Header("Power")]
    public LightSwitchQuest breaker;

    private bool isOn = false;

    public string Prompt =>
        isOn ? "ปิด ทีวี" : "เปิด ทีวี";

    void Start()
    {
        // preload กันดีเลย์
        if (tvOnSound != null) tvOnSound.LoadAudioData();
        if (tvOffSound != null) tvOffSound.LoadAudioData();

        TurnOffInstant();
    }

    public void Interact()
    {
        // 🔌 ถ้าไฟดับแล้ว ห้ามเปิดทีวี
        if (breaker != null && breaker.IsPowerOff)
            return;

        if (isOn)
            TurnOff();
        else
            TurnOn();
    }

    void TurnOn()
    {
        isOn = true;

        // 🔊 เสียงเปิดทีวี (ออกทันที)
        PlayTvSfx(true);

        if (videoPlayer != null)
            videoPlayer.Play();

        if (tvAudio != null)
            tvAudio.Play();
    }

    void TurnOff()
    {
        isOn = false;

        // 🔊 เสียงปิดทีวี
        PlayTvSfx(false);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            ClearScreen();
        }

        if (tvAudio != null)
            tvAudio.Stop();
    }

    // 🔥 เรียกจาก LightSwitchQuest เมื่อปิดเบรกเกอร์
    public void PowerCut()
    {
        TurnOff();
    }

    void TurnOffInstant()
    {
        isOn = false;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            ClearScreen();
        }

        if (tvAudio != null)
            tvAudio.Stop();
    }

    void PlayTvSfx(bool turnOn)
    {
        if (tvSfxAudio == null) return;

        tvSfxAudio.pitch = Random.Range(0.95f, 1.05f);

        if (turnOn && tvOnSound != null)
            tvSfxAudio.PlayOneShot(tvOnSound);
        else if (!turnOn && tvOffSound != null)
            tvSfxAudio.PlayOneShot(tvOffSound);
    }

    void ClearScreen()
    {
        if (videoPlayer != null && videoPlayer.targetMaterialRenderer != null)
        {
            videoPlayer.targetMaterialRenderer.material.SetTexture("_MainTex", null);
        }
    }
}
