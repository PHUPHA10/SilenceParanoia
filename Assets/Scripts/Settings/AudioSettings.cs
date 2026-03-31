using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    void Awake()
    {
        ApplySavedVolumes();
    }

    void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider != null) sfxSlider.value = sfx;
    }

    void ApplySavedVolumes()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        float musicDB = Mathf.Log10(Mathf.Clamp(music, 0.0001f, 1f)) * 20;
        float sfxDB = Mathf.Log10(Mathf.Clamp(sfx, 0.0001f, 1f)) * 20;

        mixer.SetFloat("MusicVolume", musicDB);
        mixer.SetFloat("SFXVolume", sfxDB);
    }

    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}