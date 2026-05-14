using System.Collections;
using UnityEngine;
using TMPro;

public class NPCInteract : MonoBehaviour, IInteractable
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

    [Header("NPC")]
    [SerializeField] private string npcName = "";

    [Header("UI")]
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Dialogue")]
    public DialogueLine[] dialogueLines;

    [Header("Disable While Talking")]
    public GameObject[] objectsToDisable;

    bool isTalking = false;

    public string Prompt => $"คุยกับ {npcName}";

    public void Interact()
    {
        if (isTalking)
            return;

        StartCoroutine(DialogueSequence());
    }

    IEnumerator DialogueSequence()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            yield break;

        isTalking = true;

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        foreach (var line in dialogueLines)
        {
            if (dialogueText != null)
            {
                dialogueText.text = line.text;
                dialogueText.color = line.textColor;
                dialogueText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(line.displayDuration);

            if (dialogueText != null)
                dialogueText.gameObject.SetActive(false);

            yield return new WaitForSeconds(line.delayAfter);
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        isTalking = false;
    }
}