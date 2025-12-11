using UnityEngine;

public class HidingQTEManager : MonoBehaviour
{
    public static HidingQTEManager Instance { get; private set; }

    [Header("References")]
    public hidingPlace hideSpot;    // ?????????? (????? hidingPlace) ?????
    public GameObject qteRoot;      // UI QTE ?????????

    [Header("Timing")]
    public float showDelay = 10f;   // ??????????????????? QTE ????

    public bool IsQteActive { get; private set; }

    float hideTimer = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (qteRoot != null)
            qteRoot.SetActive(false);
    }

    void Update()
    {
        if (hideSpot == null)
        {
            // Debug ?????????????
            // Debug.LogWarning("HidingQTEManager: hideSpot = null");
            return;
        }

        if (hideSpot.IsHiding)
        {
            hideTimer += Time.deltaTime;

            // Debug ??????????????????????
            // Debug.Log($"Hiding... {hideTimer:0.00}s");

            if (!IsQteActive && hideTimer >= showDelay)
            {
                ActivateQTE();
            }
        }
        else
        {
            hideTimer = 0f;

            if (IsQteActive)
                DeactivateQTE();
        }
    }

    void ActivateQTE()
    {
        IsQteActive = true;

        if (qteRoot != null)
            qteRoot.SetActive(true);

        Debug.Log("QTE Activated");
    }

    public void CompleteQTE()
    {
        DeactivateQTE();
        Debug.Log("QTE Completed");
    }

    void DeactivateQTE()
    {
        IsQteActive = false;

        if (qteRoot != null)
            qteRoot.SetActive(false);

        hideTimer = 0f;
    }
}
