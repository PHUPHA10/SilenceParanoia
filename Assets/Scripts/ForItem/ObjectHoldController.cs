using UnityEngine;

public class ObjectHoldController : MonoBehaviour
{
    [Header("Settings")]
    public float holdDistance = 2f;
    public float moveSpeed = 10f;
    public KeyCode holdKey = KeyCode.G;
    public LayerMask pickupLayer;

    private Camera cam;
    private Rigidbody heldObject;
    private bool isHolding = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(holdKey))
        {
            if (isHolding)
                DropObject();
            else
                TryPickup();
        }

        if (isHolding && heldObject != null)
        {
            MoveObject();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f, pickupLayer))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.GetComponent<Rigidbody>();

                if (heldObject != null)
                {
                    heldObject.useGravity = false;
                    heldObject.linearDamping = 10f;
                    isHolding = true;
                }
            }
        }
    }

    void MoveObject()
    {
        Vector3 targetPos = cam.transform.position + cam.transform.forward * holdDistance;
        Vector3 moveDir = targetPos - heldObject.position;

        heldObject.linearVelocity = moveDir * moveSpeed;
    }

    void DropObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject = null;
        isHolding = false;
    }
}
