using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Housewife";
    [TextArea] [SerializeField] private string[] dialogueLines;

    public string Prompt => $"Talk to {npcName}";

    public void Interact()
    {
        // ส่งข้อมูลไป DialogueManager
        DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
    }
}
