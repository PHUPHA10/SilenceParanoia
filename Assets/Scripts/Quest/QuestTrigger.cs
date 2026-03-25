using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public GameObject exploreQuest;
    public GameObject findElevatorQuest;

    public AudioSource audioSource;
    public AudioClip completeQuestSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ปิดเควสสำรวจ
            exploreQuest.SetActive(false);

            // เล่นเสียงเควสเสร็จ
            audioSource.PlayOneShot(completeQuestSound);

            // เปิดเควสใหม่
            findElevatorQuest.SetActive(true);

            // ปิด trigger กันซ้ำ
            gameObject.SetActive(false);
        }
    }
}