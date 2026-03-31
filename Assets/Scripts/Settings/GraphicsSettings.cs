using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    [Header("Brightness")]
    public Slider brightnessSlider;
    public Image brightnessOverlayGame;
    public Image brightnessOverlaySettings;

    [Header("Fullscreen")]
    public Toggle fullscreenToggle;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    // 🔥 เพิ่มตรงนี้
    void Awake()
    {
        ApplySettings(); // apply ทันทีตอนเข้า scene
    }

    void Start()
    {
        float brightnessValue = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessSlider.value = brightnessValue;
        SetBrightness(brightnessValue);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        SetupResolutions();
    }

    // 🔥 เพิ่มตรงนี้
    void ApplySettings()
    {
        float brightnessValue = PlayerPrefs.GetFloat("Brightness", 1f);
        SetBrightness(brightnessValue);

        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
        if (savedIndex != -1)
        {
            resolutions = Screen.resolutions;
            uniqueResolutions.Clear();

            for (int i = 0; i < resolutions.Length; i++)
            {
                bool exists = false;

                for (int j = 0; j < uniqueResolutions.Count; j++)
                {
                    if (uniqueResolutions[j].width == resolutions[i].width &&
                        uniqueResolutions[j].height == resolutions[i].height)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    uniqueResolutions.Add(resolutions[i]);
            }

            if (savedIndex >= 0 && savedIndex < uniqueResolutions.Count)
            {
                Resolution chosenRes = uniqueResolutions[savedIndex];
                Screen.SetResolution(chosenRes.width, chosenRes.height, isFullscreen);
            }
        }
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        uniqueResolutions.Clear();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            bool exists = false;

            for (int j = 0; j < uniqueResolutions.Count; j++)
            {
                if (uniqueResolutions[j].width == resolutions[i].width &&
                    uniqueResolutions[j].height == resolutions[i].height)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                uniqueResolutions.Add(resolutions[i]);

                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        SetResolution(savedIndex);
    }

    public void SetBrightness(float value)
    {
        float maxDarkness = 0.5f;
        float darkness = (1f - value) * maxDarkness;

        if (brightnessOverlayGame != null)
            brightnessOverlayGame.color = new Color(0, 0, 0, darkness);

        if (brightnessOverlaySettings != null)
            brightnessOverlaySettings.color = new Color(0, 0, 0, darkness);

        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= uniqueResolutions.Count)
            return;

        Resolution chosenRes = uniqueResolutions[index];

        Screen.SetResolution(
            chosenRes.width,
            chosenRes.height,
            Screen.fullScreen
        );

        PlayerPrefs.SetInt("ResolutionIndex", index);
    }
}