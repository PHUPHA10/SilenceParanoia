using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Mount (ติดกับกล้อง)")]
    [SerializeField] private Camera mountCamera;
    [SerializeField] private Vector3 localPosition = new Vector3(0f, -0.10f, 0.60f); // กลางลำตัว
    [SerializeField] private Vector3 localEuler = Vector3.zero;

    [Header("Input")]
    [SerializeField] private InputActionReference toggleAction; // ถ้าเว้น ใช้ fallbackKey
    [SerializeField] private KeyCode fallbackKey = KeyCode.F;

    [Header("Beam Settings")]
    [SerializeField] private float spotAngle = 75f;
    [SerializeField] private float range = 24f;
    [SerializeField] private float intensity = 350f;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private LightShadows shadows = LightShadows.Soft;
    [SerializeField, Range(0f, 1f)] private float shadowStrength = 0.35f;

    [Header("Soft Edge (Cookie)")]
    [SerializeField] private bool useCookie = true;
    [SerializeField, Range(64, 1024)] private int cookieSize = 256;

    [Header("Anti-Hotspot (Auto Dim Near Surface)")]
    [SerializeField] private bool autoDimNearSurface = true;
    [Tooltip("เริ่มดิมเมื่อระยะชน < ค่านี้ (เมตร)")]
    [SerializeField] private float dimStartDistance = 2.0f;   // ปรับให้มากกว่าความสูงตา/พื้น (~1.6–1.8m)
    [Tooltip("ดิมสุดเมื่อระยะชน <= ค่านี้ (เมตร)")]
    [SerializeField] private float dimEndDistance   = 0.6f;
    [SerializeField, Range(0.1f, 1f)] private float dimMinFactor = 0.25f;
    [Tooltip("ระยะสูงสุดที่ใช้เช็ค (กันกรณีสูงจากพื้นมาก ๆ)")]
    [SerializeField] private float dimMaxCheckDistance = 3.5f; // ยิงไกลพอสำหรับ plane
    [Tooltip("เริ่มยิงเลยหน้าไฟเล็กน้อย กันกรณีจ่อคอลไลเดอร์")]
    [SerializeField] private float dimRayStartOffset = 0.05f;
    [Tooltip("ใช้ SphereCast เพื่อลดพลาดผิวเรียบ")]
    [SerializeField] private bool useSphereCast = false;
    [SerializeField] private float sphereRadius = 0.05f;
    [Tooltip("เลเยอร์ที่นับเป็นพื้น/ผนัง (ถ้าไม่ตั้ง = Everything)")]
    [SerializeField] private LayerMask surfaceMask = ~0;

    private Light flashLight;
    private float baseIntensity;
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
        flashLight.range = range;
        flashLight.intensity = intensity;
        flashLight.shadows = shadows;
        flashLight.shadowStrength = shadowStrength;
        flashLight.enabled = false;

        baseIntensity = intensity;

        if (useCookie) flashLight.cookie = MakeSoftCircleCookie(cookieSize);
    }

    void OnEnable()  { if (toggleAction) toggleAction.action.Enable(); }
    void OnDisable() { if (toggleAction) toggleAction.action.Disable(); }

    void Update()
    {
        bool pressed = toggleAction ? toggleAction.action.WasPressedThisFrame()
                                    : Input.GetKeyDown(fallbackKey);
        if (pressed) Toggle();

        if (isOn && autoDimNearSurface)
            ApplyAutoDim();
    }

    void ApplyAutoDim()
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

        if (hitSomething)
        {
            float d = hit.distance;
            // d >= start -> 1 (ไม่ดิม), d <= end -> dimMinFactor (ดิมสุด)
            float t = Mathf.InverseLerp(dimEndDistance, dimStartDistance, d);
            float factor = Mathf.Lerp(dimMinFactor, 1f, t);
            flashLight.intensity = baseIntensity * factor;

            // Debug ให้ดูใน Scene
            Debug.DrawLine(origin, hit.point, Color.green);
        }
        else
        {
            flashLight.intensity = baseIntensity;
            Debug.DrawRay(origin, tr.forward * maxCheck, Color.red);
        }
    }

    public void Toggle()
    {
        isOn = !isOn;
        flashLight.enabled = isOn;
        if (isOn) flashLight.intensity = baseIntensity;
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
            float r = Mathf.Sqrt(nx * nx + ny * ny); // 0 = กลาง, ~1 = ขอบ
            float v = Mathf.Clamp01(1f - Mathf.SmoothStep(0.70f, 1.00f, r));
            tex.SetPixel(x, y, new Color(v, v, v, 1f));
        }
        tex.Apply(false, true);
        return tex;
    }
}
