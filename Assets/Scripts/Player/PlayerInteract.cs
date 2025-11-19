using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactMask = ~0;
    [SerializeField] private InputActionReference interactAction;

    [Header("UI Settings")]
    [SerializeField] private TMP_Text promptLabel;

    private IInteractable current;
    private IInteractable last;

    private OutlineItem currentoutlineitem ;
    private OutlineItem lastoutlineitem;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void OnEnable()
    {
        if (interactAction != null) interactAction.action.Enable();
    }

    void OnDisable()
    {
        if (interactAction != null) interactAction.action.Disable();
    }

    void Update()
    {
        DetectInteractable();

        // ปุ่ม E ปกติ
        if (current != null && interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            current.Interact();
            HidePrompt();
        }

        // ★ ปุ่ม Q สำหรับตาแมว
        if (current is CateyeDoor cateye && Keyboard.current.qKey.wasPressedThisFrame)
        {
            cateye.ToggleCateye();
        }
    }


    private void DetectInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, distance, interactMask);

        Debug.DrawRay(ray.origin, ray.direction * distance, hitSomething ? Color.green : Color.red);

        if (hitSomething)
        {
            // หา IInteractable
            current = hit.collider.GetComponent<IInteractable>()
                   ?? hit.collider.GetComponentInParent<IInteractable>();

            // หา OutlineItem
            currentoutlineitem = hit.collider.GetComponent<OutlineItem>()
                                ?? hit.collider.GetComponentInParent<OutlineItem>();

            // เปลี่ยนข้อความกด E เฉพาะตอนเปลี่ยน Target
            if (current != null && current != last)
            {
                ShowPrompt(current.Prompt);
            }

            // ✅ ส่วนที่แก้: จัดการเปิด/ปิดขอบให้ถูก
            if (currentoutlineitem != lastoutlineitem)
            {
                // ปิดขอบอันเก่า
                Debug.Log("Hee");
                if (lastoutlineitem != null)
                    lastoutlineitem.SetOutline(false);

                // เปิดขอบอันใหม่ (ถ้ามี)
                if (currentoutlineitem != null)
                    currentoutlineitem.SetOutline(true);
            }
        }
        else
        {
            // ไม่โดนอะไรเลย
            current = null;
            HidePrompt();

            // ปิดขอบของอันสุดท้ายที่เคยไฮไลต์ไว้
            if (lastoutlineitem != null)
                lastoutlineitem.SetOutline(false);

            currentoutlineitem = null;
            lastoutlineitem = null;
        }

        last = current;
        lastoutlineitem = currentoutlineitem;

    }


    private void ShowPrompt(string prompt)
    {
        if (promptLabel != null)
        {
            string keyText = "E";

            if (current is CateyeDoor)
                keyText = "Q";

            promptLabel.text = $"Press {keyText} to {prompt}";
            promptLabel.enabled = true;
        }
    }


    private void HidePrompt()
    {
        if (promptLabel != null)
        {
            promptLabel.text = "";
            promptLabel.enabled = false;
        }
    }

}
