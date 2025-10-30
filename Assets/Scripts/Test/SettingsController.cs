using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsController : MonoBehaviour
{
    [Header("Refs - UI")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle smoothingToggle;
    [SerializeField] private Slider smoothingSpeedSlider;
    [SerializeField] private Toggle showFpsToggle;

    [Header("Refs - Scene/Systems")]
    [SerializeField] public Volume globalVolume;
    [SerializeField] public Camera targetCamera;
    [SerializeField] private MonoBehaviour lookController; // implements ILookSettings
    [SerializeField] public GameObject fpsDisplayObj;

    [Header("Audio (optional)")]
    [SerializeField] private AudioMixer mixer;

    private ColorAdjustments colorAdj;

    // PlayerPrefs keys
    private const string PP_Brightness      = "SET_Brightness";
    private const string PP_FOV             = "SET_FOV";
    private const string PP_Sensitivity     = "SET_Sensitivity";
    private const string PP_SmoothingOn     = "SET_SmoothingOn";
    private const string PP_SmoothingSpeed  = "SET_SmoothingSpeed";
    private const string PP_ShowFPS         = "SET_ShowFPS";

    private void Awake()
    {
        if (globalVolume && globalVolume.profile)
        {
            if (!globalVolume.profile.TryGet(out colorAdj))
            {
                colorAdj = globalVolume.profile.Add<ColorAdjustments>(true);
                colorAdj.postExposure.overrideState = true;
            }
        }
    }

    private void OnEnable()
    {
        LoadFromPrefs();
        ApplyAllPreview();
        HookUIEvents();
    }

    private void OnDisable()
    {
        UnhookUIEvents();
    }

    private void HookUIEvents()
    {
        if (brightnessSlider)     brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        if (fovSlider)            fovSlider.onValueChanged.AddListener(OnFOVChanged);
        if (sensitivitySlider)    sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        if (smoothingToggle)      smoothingToggle.onValueChanged.AddListener(OnSmoothingToggled);
        if (smoothingSpeedSlider) smoothingSpeedSlider.onValueChanged.AddListener(OnSmoothingSpeedChanged);
        if (showFpsToggle)        showFpsToggle.onValueChanged.AddListener(OnShowFpsToggled);
    }

    private void UnhookUIEvents()
    {
        if (brightnessSlider)     brightnessSlider.onValueChanged.RemoveListener(OnBrightnessChanged);
        if (fovSlider)            fovSlider.onValueChanged.RemoveListener(OnFOVChanged);
        if (sensitivitySlider)    sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
        if (smoothingToggle)      smoothingToggle.onValueChanged.RemoveListener(OnSmoothingToggled);
        if (smoothingSpeedSlider) smoothingSpeedSlider.onValueChanged.RemoveListener(OnSmoothingSpeedChanged);
        if (showFpsToggle)        showFpsToggle.onValueChanged.RemoveListener(OnShowFpsToggled);
    }

    // ---- Preview handlers ----
    private void OnBrightnessChanged(float v)
    {
        if (colorAdj != null) colorAdj.postExposure.value = v;
    }

    private void OnFOVChanged(float v)
    {
        if (targetCamera) targetCamera.fieldOfView = v;
    }

    private void OnSensitivityChanged(float v)
    {
        var bridge = lookController as ILookSettings;
        if (bridge != null) bridge.SetLookSensitivity(v);
    }

    private void OnSmoothingToggled(bool on)
    {
        var bridge = lookController as ILookSettings;
        if (bridge != null) bridge.SetSmoothingEnabled(on);
        if (smoothingSpeedSlider) smoothingSpeedSlider.interactable = on;
    }

    private void OnSmoothingSpeedChanged(float v)
    {
        var bridge = lookController as ILookSettings;
        if (bridge != null) bridge.SetSmoothingSpeed(v);
    }

    private void OnShowFpsToggled(bool on)
    {
        if (fpsDisplayObj) fpsDisplayObj.SetActive(on);
    }

    // ---- Apply / Save / Load ----
    public void ApplyAndSave()
    {
        if (brightnessSlider)     PlayerPrefs.SetFloat(PP_Brightness,     brightnessSlider.value);
        if (fovSlider)            PlayerPrefs.SetFloat(PP_FOV,            fovSlider.value);
        if (sensitivitySlider)    PlayerPrefs.SetFloat(PP_Sensitivity,    sensitivitySlider.value);
        if (smoothingToggle)      PlayerPrefs.SetInt  (PP_SmoothingOn,    smoothingToggle.isOn ? 1 : 0);
        if (smoothingSpeedSlider) PlayerPrefs.SetFloat(PP_SmoothingSpeed, smoothingSpeedSlider.value);
        if (showFpsToggle)        PlayerPrefs.SetInt  (PP_ShowFPS,        showFpsToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ResetToDefault()
    {
        if (brightnessSlider)     brightnessSlider.value     = 0.25f;
        if (fovSlider)            fovSlider.value            = 60f;
        if (sensitivitySlider)    sensitivitySlider.value    = 1.2f;
        if (smoothingToggle)      smoothingToggle.isOn       = true;
        if (smoothingSpeedSlider) smoothingSpeedSlider.value = 8f;
        if (showFpsToggle)        showFpsToggle.isOn         = false;

        ApplyAllPreview();
    }

    private void LoadFromPrefs()
    {
        if (brightnessSlider)     brightnessSlider.value     = PlayerPrefs.GetFloat(PP_Brightness,     0.25f);
        if (fovSlider)            fovSlider.value            = PlayerPrefs.GetFloat(PP_FOV,            60f);
        if (sensitivitySlider)    sensitivitySlider.value    = PlayerPrefs.GetFloat(PP_Sensitivity,    1.2f);
        if (smoothingToggle)      smoothingToggle.isOn       = PlayerPrefs.GetInt  (PP_SmoothingOn,    1) == 1;
        if (smoothingSpeedSlider) smoothingSpeedSlider.value = PlayerPrefs.GetFloat(PP_SmoothingSpeed, 8f);
        if (showFpsToggle)        showFpsToggle.isOn         = PlayerPrefs.GetInt  (PP_ShowFPS,        0) == 1;
    }

    private void ApplyAllPreview()
    {
        if (brightnessSlider)     OnBrightnessChanged(brightnessSlider.value);
        if (fovSlider)            OnFOVChanged(fovSlider.value);
        if (sensitivitySlider)    OnSensitivityChanged(sensitivitySlider.value);
        if (smoothingToggle)      OnSmoothingToggled(smoothingToggle.isOn);
        if (smoothingSpeedSlider) OnSmoothingSpeedChanged(smoothingSpeedSlider.value);
        if (showFpsToggle)        OnShowFpsToggled(showFpsToggle.isOn);
    }

    public void ClosePanel(GameObject panel) { if (panel) panel.SetActive(false); }
}

public interface ILookSettings
{
    void SetLookSensitivity(float value);
    void SetSmoothingEnabled(bool on);
    void SetSmoothingSpeed(float speed);
}
