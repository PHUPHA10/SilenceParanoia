using UnityEngine;
using TMPro;

public class RoomExploreQuest : MonoBehaviour
{
    [Header("Rooms")]
    public Collider kitchenTrigger;
    public Collider bathroomTrigger;

    [Header("UI")]
    public TMP_Text kitchenText;
    public TMP_Text bathroomText;
    public GameObject exploreGroup;
    public GameObject breakerQuestGroup;

    [Header("Breaker")]
    public LightSwitchQuest breaker;

    [Header("Audio")]
    public AudioSource questAudio;          // 🔔 AudioSource กลาง
    public AudioClip exploreCompleteSound;  // 🔔 เสียงจบเควสสำรวจ

    private bool kitchenVisited = false;
    private bool bathroomVisited = false;

    void Start()
    {
        UpdateUI();

        if (breaker != null)
            breaker.SetLocked(true);

        if (exploreGroup != null)
            exploreGroup.SetActive(true);

        if (breakerQuestGroup != null)
            breakerQuestGroup.SetActive(false);
    }

    public void OnEnterRoom(Collider room)
    {
        if (room == kitchenTrigger)
            kitchenVisited = true;

        if (room == bathroomTrigger)
            bathroomVisited = true;

        UpdateUI();

        if (kitchenVisited && bathroomVisited)
            CompleteExploreQuest();
    }

    void UpdateUI()
    {
        if (kitchenText != null)
            kitchenText.text = $"ตรวจสอบห้องครัว {(kitchenVisited ? "1/1" : "0/1")}";

        if (bathroomText != null)
            bathroomText.text = $"ตรวจสอบห้องน้ำ {(bathroomVisited ? "1/1" : "0/1")}";
    }

    void CompleteExploreQuest()
    {
        if (exploreGroup != null)
            exploreGroup.SetActive(false);

        if (breakerQuestGroup != null)
            breakerQuestGroup.SetActive(true);

        if (breaker != null)
            breaker.SetLocked(false);

        // 🔔 เล่นเสียงจบเควสสำรวจ
        if (questAudio != null && exploreCompleteSound != null)
            questAudio.PlayOneShot(exploreCompleteSound);
    }
}
