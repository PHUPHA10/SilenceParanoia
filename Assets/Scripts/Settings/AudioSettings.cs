using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        SetMusicVolume(music);
        SetSFXVolume(sfx);
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
