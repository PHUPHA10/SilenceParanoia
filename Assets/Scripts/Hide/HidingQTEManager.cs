using UnityEngine;

public class HidingQTEManager : MonoBehaviour
{
    public static HidingQTEManager Instance { get; private set; }

    [Header("References")]
    public GameObject qteRoot;

    [Header("QTE Games")]
    public GameObject[] qteGames;

    [Header("Timing")]
    public float showDelay = 10f;
    public float switchInterval = 15f;

    public bool IsQteActive { get; private set; }

    private hidingPlace currentHideSpot;
    public hidingPlace CurrentHideSpot => currentHideSpot;

    private float hideTimer = 0f;
    private float qteTimer = 0f;

    private GameObject activeQTE;
    private int lastIndex = -1;

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

        foreach (var qte in qteGames)
            if (qte != null)
                qte.SetActive(false);
    }

    void Update()
    {
        if (currentHideSpot == null || !currentHideSpot.IsHiding)
        {
            hideTimer = 0f;
            qteTimer = 0f;
            return;
        }

        if (!IsQteActive)
        {
            hideTimer += Time.deltaTime;

            if (hideTimer >= showDelay)
                ActivateQTE();
        }
        else
        {
            qteTimer += Time.deltaTime;

            if (qteTimer >= switchInterval)
                SwitchQTE();
        }
    }


    public void RegisterHideSpot(hidingPlace spot)
    {

        ForceStopQTE();

        currentHideSpot = spot;
        hideTimer = 0f;
        qteTimer = 0f;

        Debug.Log("REGISTER HIDE SPOT: " + spot.gameObject.name);
    }

    public void UnregisterHideSpot(hidingPlace spot)
    {
        if (currentHideSpot == spot)
        {
            ForceStopQTE();
            currentHideSpot = null;
        }
    }


    public void OnQTEFail()
    {
        ForceStopQTE();
    }

    void ActivateQTE()
    {
        IsQteActive = true;
        qteTimer = 0f;

        if (qteRoot != null)
            qteRoot.SetActive(true);

        SwitchQTE();
    }

    void SwitchQTE()
    {
        foreach (var qte in qteGames)
            if (qte != null)
                qte.SetActive(false);

        int index;
        do
        {
            index = Random.Range(0, qteGames.Length);
        }
        while (index == lastIndex && qteGames.Length > 1);

        lastIndex = index;

        activeQTE = qteGames[index];
        activeQTE.SetActive(true);

        qteTimer = 0f;


    }

    public void ForceStopQTE()
    {
        IsQteActive = false;
        hideTimer = 0f;
        qteTimer = 0f;

        if (activeQTE != null)
            activeQTE.SetActive(false);

        if (qteRoot != null)
            qteRoot.SetActive(false);
    }
}
