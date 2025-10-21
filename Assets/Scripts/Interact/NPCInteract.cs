using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Housewife";


    // กำหนดบทพูด (string ธรรมดา)
    [TextArea]
    [SerializeField] private string[] dialogueLines;

    // เลือกว่าจะใช้เวลาเท่ากันทุกบรรทัด หรือกำหนดเวลาต่อบรรทัด
    [Header("Timing")]
    [SerializeField] private bool usePerLineDurations = false;
    [SerializeField] private float secondsPerLine = 2.5f;
    [SerializeField] private float[] perLineDurations; // ต้องมีขนาดเท่ากับ dialogueLines

    public string Prompt => $"Talk to {npcName}";

    public void Interact()
    {
        if (DialogueManager.Instance == null) return;

        if (usePerLineDurations && perLineDurations != null && perLineDurations.Length == dialogueLines.Length)
        {
            // โหมด: เวลาต่อบรรทัด
            DialogueManager.Instance.StartDialogue(npcName, dialogueLines, perLineDurations);
        }
        else
        {
            // โหมด: เวลาเท่ากันทุกบรรทัด
            DialogueManager.Instance.StartDialogue(npcName, dialogueLines, secondsPerLine);
        }
        dialogueLines = new string[] {
            "Hello dear, did you have lunch yet?",
            "Be careful, it's getting dark outside.",
            "Don't forget to bring your flashlight."
};
    }
    
}
