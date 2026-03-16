using UnityEngine;
using TMPro;

public class PointerController : MonoBehaviour
{
    [Header("References")]
    public GameObject heartBeatBG;
    public RectTransform pointer;
    public RectTransform pointA;
    public RectTransform pointB;
    public RectTransform safeZone;
    public TMP_Text failText;

    [Header("Timing")]
    public float bgShowDelay = 3f;       // รอก่อนให้ BG โผล่
    public float pointerShowDelay = 3.4f;  // หลัง BG โผล่ รอก่อนให้ pointer โผล่

    [Header("Pointer Settings")]
    public float moveSpeed = 120f;
    public float inputWindowTime = 5f;

    [Header("Fail Settings")]
    public int maxFails = 3;

    private Vector2 startPos;
    private Vector2 endPos;

    private int failCount;
    private float stateTimer;
    private float roundTimer;

    private bool qteActive;
    private bool pointerMoving;
    private bool bgVisible;

    private enum QTEState
    {
        WaitingForBG,
        WaitingForPointer,
        PointerMoving
    }

    private QTEState currentState;

    private void Awake()
    {
        SetBG(false);
        SetPointer(false);
    }

    private void OnEnable()
    {
        StartQTE();
    }

    private void Update()
    {
        if (!qteActive) return;

        stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case QTEState.WaitingForBG:
                if (stateTimer >= bgShowDelay)
                {
                    SetBG(true);
                    bgVisible = true;
                    stateTimer = 0f;
                    currentState = QTEState.WaitingForPointer;
                }
                break;

            case QTEState.WaitingForPointer:
                if (stateTimer >= pointerShowDelay)
                {
                    SpawnPointer();
                }
                break;

            case QTEState.PointerMoving:
                UpdatePointerMovement();
                break;
        }
    }

    public void StartQTE()
    {
        UpdateText();

        qteActive = true;
        BeginNextRoundDelay();
    }

    private void BeginNextRoundDelay()
    {
        pointerMoving = false;
        bgVisible = false;

        roundTimer = 0f;
        stateTimer = 0f;

        SetBG(false);
        SetPointer(false);

        currentState = QTEState.WaitingForBG;
    }

    private void SpawnPointer()
    {
        stateTimer = 0f;
        roundTimer = 0f;
        pointerMoving = true;
        currentState = QTEState.PointerMoving;

        bool startFromA = Random.value > 0.5f;

        if (startFromA)
        {
            startPos = pointA.anchoredPosition;
            endPos = pointB.anchoredPosition;
        }
        else
        {
            startPos = pointB.anchoredPosition;
            endPos = pointA.anchoredPosition;
        }

        pointer.anchoredPosition = startPos;
        SetPointer(true);
    }

    private void UpdatePointerMovement()
    {
        roundTimer += Time.deltaTime;

        pointer.anchoredPosition = Vector2.MoveTowards(
            pointer.anchoredPosition,
            endPos,
            moveSpeed * Time.deltaTime
        );

        // ถึงปลาย = fail
        if (Vector2.Distance(pointer.anchoredPosition, endPos) <= 0.01f)
        {
            RegisterFail();
            if (qteActive) BeginNextRoundDelay();
            return;
        }

        // หมดเวลา = fail
        if (roundTimer >= inputWindowTime)
        {
            RegisterFail();
            if (qteActive) BeginNextRoundDelay();
            return;
        }

        // กด
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool safe = RectTransformUtility.RectangleContainsScreenPoint(
                safeZone,
                RectTransformUtility.WorldToScreenPoint(null, pointer.position)
            );

            if (!safe)
                RegisterFail();

            if (qteActive) BeginNextRoundDelay();
        }
    }

    private void SetBG(bool visible)
    {
        if (heartBeatBG != null)
            heartBeatBG.SetActive(visible);
    }

    private void SetPointer(bool visible)
    {
        if (pointer != null)
            pointer.gameObject.SetActive(visible);
    }

    private void RegisterFail()
    {
        failCount++;
        UpdateText();

        if (failCount >= maxFails)
        {
            Fail();
        }
    }

    private void UpdateText()
    {
        if (failText != null)
            failText.text = $"จำนวนครั้งที่พลาด {failCount} / {maxFails}";
    }

    private void Fail()
    {
        qteActive = false;
        pointerMoving = false;

        SetBG(false);
        SetPointer(false);

        if (HidingQTEManager.Instance != null &&
            HidingQTEManager.Instance.CurrentHideSpot != null)
        {
            HidingQTEManager.Instance.CurrentHideSpot.ForceExitHide();
        }

        gameObject.SetActive(false);
    }
}