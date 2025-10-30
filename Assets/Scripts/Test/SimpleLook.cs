using UnityEngine;

public class SimpleLook : MonoBehaviour, ILookSettings
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float sensitivity = 1.2f;
    [SerializeField] private bool smoothing = true;
    [SerializeField] private float smoothingSpeed = 8f;

    private float xRot;
    private Vector2 targetDelta;

    void Update()
    {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity;

        if (smoothing)
        {
            targetDelta = Vector2.Lerp(targetDelta, delta, 1 - Mathf.Exp(-smoothingSpeed * Time.deltaTime));
            delta = targetDelta;
        }

        xRot -= delta.y;
        xRot = Mathf.Clamp(xRot, -89f, 89f);
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        if (playerBody) playerBody.Rotate(Vector3.up * delta.x);
    }

    public void SetLookSensitivity(float value) => sensitivity = value;
    public void SetSmoothingEnabled(bool on)     => smoothing = on;
    public void SetSmoothingSpeed(float speed)   => smoothingSpeed = speed;
}
