using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Mount (ติดกับกล้อง)")]
    [SerializeField] private Camera mountCamera;
    [SerializeField] private Vector3 localPosition = new Vector3(0f, -0.10f, 0.60f);
    [SerializeField] private Vector3 localEuler = Vector3.zero;

    [Header("Input")]
    [SerializeField] private InputActionReference toggleAction;
    [SerializeField] private KeyCode fallbackKey = KeyCode.F;

    [Header("Beam Settings")]
    [SerializeField] private float spotAngle = 75f;
    [SerializeField, Range(0f, 1f)] private float innerSpotRatio = 0.85f;
    [SerializeField] private float range = 21f;
    [SerializeField] private float intensity = 200f;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private LightShadows shadows = LightShadows.Soft;
    [SerializeField, Range(0f, 1f)] private float shadowStrength = 0.35f;

    [Header("Soft Edge (Cookie)")]
    [SerializeField] private bool useCookie = true;
    [SerializeField, Range(64, 1024)] private int cookieSize = 350
    ;

    [Header("Auto-Dim (หรี่แสงเมื่อใกล้ผิว)")]
    [SerializeField] private bool autoDimNearSurface = true;
    [SerializeField] private float dimStartDistance = 1.2f;
    [SerializeField] private float dimEndDistance   = 0.3f;
    [SerializeField, Range(0.1f, 1f)] private float dimMinFactor = 0.25f;
    [SerializeField] private float dimMaxCheckDistance = 3.5f;
    [SerializeField] private float dimRayStartOffset = 0.05f;
    [SerializeField] private bool useSphereCast = false;
    [SerializeField] private float sphereRadius = 0.05f;
    [SerializeField] private LayerMask surfaceMask = ~0;

    [Header("Auto-Widen (กระจายลำแสงเมื่อใกล้ผิว)")]
    [SerializeField] private bool autoWiden = true;
    [SerializeField] private float widenStartDistance = 1.5f;
    [SerializeField] private float widenEndDistance   = 0.25f;
    [SerializeField, Range(1f, 2f)] private float maxWidenMultiplier = 1.6f;
    [SerializeField] private bool conserveBrightness = true;
    [SerializeField] private float widenLerpSpeed = 10f;

    private Light flashLight;
    private float baseIntensity;
    private float baseSpotAngle;
    private float baseInnerSpotAngle;
    private bool isOn;

    void Awake()
    {
        if (!mountCamera) mountCamera = Camera.main;

        var go = new GameObject("Flashlight");
        go.transform.SetParent(mountCamera.transform, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.Euler(localEuler);

        flashLight = go.AddComponent<Light>();
        flashLight.type = LightType.Spot;
        flashLight.color = color;
        flashLight.spotAngle = spotAngle;
        flashLight.innerSpotAngle = Mathf.Clamp(spotAngle * innerSpotRatio, 1f, spotAngle);
        flashLight.range = range;
        flashLight.intensity = intensity;
        flashLight.shadows = shadows;
        flashLight.shadowStrength = shadowStrength;
        flashLight.enabled = false;

        baseIntensity = intensity;
        baseSpotAngle = flashLight.spotAngle;
        baseInnerSpotAngle = flashLight.innerSpotAngle;

        if (useCookie) flashLight.cookie = MakeSoftCircleCookie(cookieSize);
    }

    void OnEnable()  { if (toggleAction) toggleAction.action.Enable(); }
    void OnDisable() { if (toggleAction) toggleAction.action.Disable(); }

    void Update()
    {
        bool pressed = toggleAction ? toggleAction.action.WasPressedThisFrame()
                                    : Input.GetKeyDown(fallbackKey);
        if (pressed) Toggle();

        if (isOn && (autoDimNearSurface || autoWiden))
            ApplyNearSurfaceAdjust();
    }

    void ApplyNearSurfaceAdjust()
    {
        var tr = flashLight.transform;
        Vector3 origin = tr.position + tr.forward * dimRayStartOffset;
        float maxCheck = Mathf.Max(dimMaxCheckDistance, dimStartDistance + 0.1f);

        bool hitSomething;
        RaycastHit hit;

        if (useSphereCast)
            hitSomething = Physics.SphereCast(origin, sphereRadius, tr.forward, out hit, maxCheck, surfaceMask, QueryTriggerInteraction.Collide);
        else
            hitSomething = Physics.Raycast(origin, tr.forward, out hit, maxCheck, surfaceMask, QueryTriggerInteraction.Collide);

        float targetIntensity = baseIntensity;
        float targetSpot = baseSpotAngle;
        float targetInner = baseInnerSpotAngle;

        if (hitSomething)
        {
            // Auto-Dim
            if (autoDimNearSurface)
            {
                float d = hit.distance;
                float t = Mathf.InverseLerp(dimEndDistance, dimStartDistance, d);
                float factor = Mathf.Lerp(dimMinFactor, 1f, t);
                targetIntensity = baseIntensity * factor;
            }

            // Auto-Widen
            if (autoWiden)
            {
                float d = hit.distance;
                float wT = Mathf.InverseLerp(widenStartDistance, widenEndDistance, d);
                wT = Mathf.Clamp01(1f - wT);
                float widened = baseSpotAngle * Mathf.Lerp(1f, maxWidenMultiplier, wT);
                targetSpot  = widened;
                targetInner = Mathf.Clamp(widened * innerSpotRatio, 1f, widened);

                if (conserveBrightness)
                {
                    float areaScale = (baseSpotAngle * baseSpotAngle) / (widened * widened);
                    targetIntensity *= areaScale;
                }
            }

            Debug.DrawLine(origin, hit.point, Color.green);
        }
        else
        {
            Debug.DrawRay(origin, tr.forward * maxCheck, Color.red);
        }

        // Lerp ให้นุ่ม
        flashLight.intensity      = Mathf.Lerp(flashLight.intensity,      targetIntensity, Time.deltaTime * widenLerpSpeed);
        flashLight.spotAngle      = Mathf.Lerp(flashLight.spotAngle,      targetSpot,      Time.deltaTime * widenLerpSpeed);
        flashLight.innerSpotAngle = Mathf.Lerp(flashLight.innerSpotAngle, targetInner,     Time.deltaTime * widenLerpSpeed);
    }

    public void Toggle()
    {
        isOn = !isOn;
        flashLight.enabled = isOn;
        if (isOn)
        {
            flashLight.intensity = baseIntensity;
            flashLight.spotAngle = baseSpotAngle;
            flashLight.innerSpotAngle = baseInnerSpotAngle;
        }
    }

    public void AttachTo(Camera newCam)
    {
        if (!newCam || !flashLight) return;
        mountCamera = newCam;
        flashLight.transform.SetParent(newCam.transform, false);
        flashLight.transform.localPosition = localPosition;
        flashLight.transform.localRotation = Quaternion.Euler(localEuler);
    }

    // ===== Helpers =====
    Texture2D MakeSoftCircleCookie(int size)
    {
        var tex = new Texture2D(size, size, TextureFormat.R8, false, true);
        tex.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float nx = (x + 0.5f) / size * 2f - 1f;
            float ny = (y + 0.5f) / size * 2f - 1f;
            float r = Mathf.Sqrt(nx * nx + ny * ny);
            float v = Mathf.Clamp01(1f - Mathf.SmoothStep(0.70f, 1.00f, r));
            tex.SetPixel(x, y, new Color(v, v, v, 1f));
        }
        tex.Apply(false, true);
        return tex;
    }
}
