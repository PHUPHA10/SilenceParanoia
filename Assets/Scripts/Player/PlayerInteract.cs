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

        if (current != null && interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            current.Interact();
            HidePrompt();
        }
    }

    private void DetectInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, distance, interactMask);

        Debug.DrawRay(ray.origin, ray.direction * distance, hitSomething ? Color.green : Color.red);

        if (hitSomething)
        {
            current = hit.collider.GetComponent<IInteractable>()
                   ?? hit.collider.GetComponentInParent<IInteractable>();

            currentoutlineitem = hit.collider.GetComponent<OutlineItem>()
            ?? hit.collider.GetComponentInParent<OutlineItem>();    

            if (current != null && current != last)
            {
                ShowPrompt(current.Prompt);

            }
            if (currentoutlineitem != null && currentoutlineitem != lastoutlineitem)
            {
                currentoutlineitem.SetOutlineScale(1.04f);
            }
        }
        else
        {
            current = null;
            HidePrompt();
            currentoutlineitem?.SetOutlineScale(0);
            currentoutlineitem = null;
        }


        last = current;
        lastoutlineitem = currentoutlineitem;
    }

    private void ShowPrompt(string prompt)
    {
        if (promptLabel != null)
        {
            // แก้เป็นภาษาอังกฤษ
            promptLabel.text = $"Press E to {prompt}";
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
