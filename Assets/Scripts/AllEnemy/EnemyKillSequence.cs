using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class EnemyKillSequence : MonoBehaviour
{
    [Header("Enemy")]
    public Animator enemyAnimator;
    public string killAnimationName = "Action_gun"; 

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas; 

    bool isPlaying = false;

    public void PlayKillSequence()
    {
        if (isPlaying) return;
        isPlaying = true;

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // ▶ เล่นอนิเมชันฆ่า
        if (enemyAnimator != null)
        {
            enemyAnimator.Play(killAnimationName);
        }

        // ⏱ รอให้ Animation เล่นจบ
        float animLength = 1.0f;
        if (enemyAnimator != null)
        {
            AnimatorStateInfo info = enemyAnimator.GetCurrentAnimatorStateInfo(0);
            animLength = info.length;
        }

        yield return new WaitForSeconds(animLength);

        // ▶ เปิดวิดีโอ
        if (videoCanvas != null)
            videoCanvas.SetActive(true);

        if (videoPlayer != null)
            videoPlayer.Play();
    }
}
