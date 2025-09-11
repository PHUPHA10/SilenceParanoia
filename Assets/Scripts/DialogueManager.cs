using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private string[] lines;
    private int currentLine;
    private bool isActive;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string npcName, string[] dialogueLines)
    {
        if (isActive) return;

        nameText.text = npcName;
        lines = dialogueLines;
        currentLine = 0;
        isActive = true;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.E))
        {
            currentLine++;
            if (currentLine < lines.Length)
                ShowLine();
            else
                EndDialogue();
        }
    }

    void ShowLine()
    {
        dialogueText.text = lines[currentLine];
    }

    void EndDialogue()
    {
        isActive = false;
        dialoguePanel.SetActive(false);
    }
}
