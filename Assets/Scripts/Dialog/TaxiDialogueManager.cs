using System.Collections;
using UnityEngine;
using TMPro;

public class TaxiDialogueManager : MonoBehaviour
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
    public float delayBeforeChoice = 1f;

    [Header("Phase 1 Dialogue (ของเดิม)")]
    public DialogueLine[] lines;

    [Header("Refuse Dialogue")]
    public DialogueLine[] refuseLines;

    TaxiChoiceManager choiceManager;

    [Header("No Stop Car Dialogue")]
    public DialogueLine[] noStopLines;

    [Header("Stop Car Dialogue")]
    public DialogueLine[] stopCarLines;

    [Header("Next Scene")]
    public string nextSceneName = "Lobby";

    // 🔥 เพิ่ม
    bool skipRequested = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        choiceManager = FindObjectOfType<TaxiChoiceManager>();

        StartPhase1();
    }

    // 🔥 เพิ่ม
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipRequested = true;
        }
    }

    public void StartPhase1()
    {
        StopAllCoroutines();
        StartCoroutine(DialogueSequence(lines, true));
    }

    public void StartRefusePhase()
    {
        Debug.Log("Start Refuse Phase");
        StopAllCoroutines();
        StartCoroutine(DialogueSequence(refuseLines, false));
    }

    IEnumerator WaitOrSkip(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (skipRequested)
            {
                skipRequested = false;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DialogueSequence(DialogueLine[] targetLines, bool showWaterChoice)
    {
        if (targetLines == null || targetLines.Length == 0)
            yield break;

        yield return new WaitForSeconds(delayBeforeStart);

        dialoguePanel.SetActive(true);

        foreach (var line in targetLines)
        {
            dialogueText.text = line.text;
            dialogueText.color = line.textColor;
            dialogueText.gameObject.SetActive(true);

            yield return StartCoroutine(WaitOrSkip(Mathf.Max(0.1f, line.displayDuration)));

            dialogueText.gameObject.SetActive(false);

            yield return StartCoroutine(WaitOrSkip(Mathf.Max(0.1f, line.delayAfter)));
        }

        dialoguePanel.SetActive(false);

        yield return new WaitForSeconds(delayBeforeChoice);

        if (showWaterChoice)
            choiceManager?.ShowWaterChoice();
        else
            choiceManager?.ShowStopCarChoice();
    }

    public void StartNoStopPhase()
    {
        StopAllCoroutines();
        StartCoroutine(DialogueAndLoadScene());
    }

    public void StartStopCarPhase()
    {
        StopAllCoroutines();
        StartCoroutine(DialogueStopCarAndLoad());
    }

    IEnumerator DialogueStopCarAndLoad()
    {
        if (stopCarLines == null || stopCarLines.Length == 0)
            yield break;

        yield return new WaitForSeconds(delayBeforeStart);

        dialoguePanel.SetActive(true);

        foreach (var line in stopCarLines)
        {
            dialogueText.text = line.text;
            dialogueText.color = line.textColor;
            dialogueText.gameObject.SetActive(true);

            yield return StartCoroutine(WaitOrSkip(Mathf.Max(0.1f, line.displayDuration)));

            dialogueText.gameObject.SetActive(false);

            yield return StartCoroutine(WaitOrSkip(Mathf.Max(0.1f, line.delayAfter)));
        }

        dialoguePanel.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }

    IEnumerator DialogueAndLoadScene()
    {
        if (noStopLines == null || noStopLines.Length == 0)
            yield break;

        yield return new WaitForSeconds(delayBeforeStart);

        dialoguePanel.SetActive(true);

        foreach (var line in noStopLines)
        {
            dialogueText.text = line.text;
            dialogueText.color = line.textColor;
            dialogueText.gameObject.SetActive(true);

            yield return StartCoroutine(WaitOrSkip(Mathf.Max(0.1f, line.displayDuration)));

            dialogueText.gameObject.SetActive(false);

            yield return StartCoroutine(WaitOrSkip(Mathf.Max(0.1f, line.delayAfter)));
        }

        dialoguePanel.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("Lobby");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found!");
        }
    }
}