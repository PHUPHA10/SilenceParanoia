using System.Collections;
using UnityEngine;
using TMPro;

public class SimpleDialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea]
        public string text;

        public Color textColor = Color.white;
        public float displayDuration = 4f;
        public float delayAfter = 1f;
    }

    [Header("UI")]
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Timing")]
    public float delayBeforeStart = 0.2f;
    public float delayBeforeEnd = 1f;

    [Header("Dialogue")]
    public DialogueLine[] lines;

    bool skipRequested = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        StartDialogue();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipRequested = true;
        }
    }

    public void StartDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(DialogueSequence());
    }

    IEnumerator DialogueSequence()
    {
        if (lines == null || lines.Length == 0)
            yield break;

        yield return new WaitForSeconds(delayBeforeStart);

        dialoguePanel.SetActive(true);

        foreach (var line in lines)
        {
            skipRequested = false;

            dialogueText.text = line.text;
            dialogueText.color = line.textColor;
            dialogueText.gameObject.SetActive(true);

            float timer = 0f;

            while (timer < line.displayDuration)
            {
                if (skipRequested)
                    break;

                timer += Time.deltaTime;
                yield return null;
            }

            dialogueText.gameObject.SetActive(false);

            timer = 0f;

            while (timer < line.delayAfter)
            {
                if (skipRequested)
                    break;

                timer += Time.deltaTime;
                yield return null;
            }
        }

        dialoguePanel.SetActive(false);

        yield return new WaitForSeconds(delayBeforeEnd);

        OnDialogueEnd();
    }

    void OnDialogueEnd()
    {
        Debug.Log("Dialogue End");
    }
}