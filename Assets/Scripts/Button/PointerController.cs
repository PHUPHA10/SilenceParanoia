using UnityEngine;
using TMPro;

public class PointerController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public float moveSpeed = 100f;

    public int maxFails = 3;
    public float inputWindowTime = 3f;
    public TMP_Text failText;

    RectTransform pointer;
    Vector3 target;
    int failCount;
    float timer;
    bool active;

    void OnEnable()
    {
        pointer = GetComponent<RectTransform>();
        target = pointB.position;
        failCount = 0;
        active = true;
        ResetWindow();
        UpdateText();
    }

    void Update()
    {
        if (!active) return;

        pointer.position = Vector3.MoveTowards(pointer.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointer.position, pointA.position) < 0.1f) target = pointB.position;
        if (Vector3.Distance(pointer.position, pointB.position) < 0.1f) target = pointA.position;

        timer += Time.deltaTime;

        if (timer >= inputWindowTime)
        {
            RegisterFail();
            ResetWindow();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool safe = RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointer.position);
            if (!safe) RegisterFail();
            ResetWindow();
        }
    }

    void ResetWindow()
    {
        timer = 0f;
    }

    void RegisterFail()
    {
        failCount++;
        UpdateText();

        if (failCount >= maxFails)
            Fail();
    }

    void UpdateText()
    {
        if (failText)
            failText.text = $"จำนวนครั้งที่พลาด {failCount} / {maxFails}";
    }

    void Fail()
    {
        active = false;

        if (HidingQTEManager.Instance != null &&
            HidingQTEManager.Instance.CurrentHideSpot != null)
        {
            HidingQTEManager.Instance.CurrentHideSpot.ForceExitHide();
        }

        gameObject.SetActive(false);
    }
}
