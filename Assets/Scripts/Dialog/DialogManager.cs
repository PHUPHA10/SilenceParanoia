using UnityEngine;
using TMPro;
using System.Collections;

public enum DialogChannel
{
    Killer,
    PlayerThought
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("Killer")]
    public TMP_Text killerText;
    public float killerDisplayTime = 4f;

    [Header("Player Thought")]
    public TMP_Text thoughtText;
    public float thoughtDisplayTime = 3f;

    Coroutine currentRoutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDialog(string text, DialogChannel channel)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DialogRoutine(text, channel));
    }

    IEnumerator DialogRoutine(string text, DialogChannel channel)
    {
        TMP_Text targetText;
        float time;

        if (channel == DialogChannel.Killer)
        {
            targetText = killerText;
            time = killerDisplayTime;
        }
        else
        {
            targetText = thoughtText;
            time = thoughtDisplayTime;
        }

        targetText.text = text;
        targetText.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        targetText.gameObject.SetActive(false);
    }
}
