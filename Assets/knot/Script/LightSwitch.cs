using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LightSwitch : MonoBehaviour
{

    [Header("Switch Settings")]
    public Light[] controlledLights;
    public bool isOn = false;

    [Header("Raycast Settings")]
    public float interactDistance = 1f;
    public KeyCode toggleKey = KeyCode.E;
    

    [Header("UI Hint (optional)")]
    public GameObject interactHint;

    public LayerMask layerMask;
    Hightlight outline;
    public Camera playerCamera;
    private bool isLookingAtThis = false;

    void Start()
    {
        playerCamera = Camera.main;


        UpdateLights();

        if (interactHint != null)
            interactHint.SetActive(false);

    }

    void Update()
    {
        CheckRaycast();

        if (isLookingAtThis && Input.GetKeyDown(toggleKey))
         {
           Toggle();
         }
    }
    
    
   
    void CheckRaycast()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, layerMask))
        {
            if (hit.transform.TryGetComponent(out Hightlight outline))
            {
                if (this.outline != outline)
                {
                    this.outline?.Outline(false);
                }

                this.outline = outline;
                outline.Outline(true);
            }
            else
            {
                this.outline?.Outline(false);
                this.outline = default;
            }
        }
        else
        {
            outline?.Outline(false);
            outline = default;
        }
        
        if (Physics.Raycast(ray, out hit, interactDistance))
        {   
            if (hit.collider.gameObject == this.gameObject)
            {

                isLookingAtThis = true;
               
                    if (interactHint != null)
                    interactHint.SetActive(true);
            }

            else
            {
                ClearLook();
            }
        }
        else
        {
            ClearLook();
        }
    }

    void ClearLook()
    { 
        isLookingAtThis = false;
      

        if (interactHint != null)
            interactHint.SetActive(false);
    }
        

    
    public void Toggle()
    {
        isOn = !isOn;
        UpdateLights();
    }

    void UpdateLights()
    {
        foreach (Light light in controlledLights)
        {
            if (light != null)
                light.enabled = isOn;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;
        Gizmos.color = isLookingAtThis ? Color.green : Color.red;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance);
    }
}