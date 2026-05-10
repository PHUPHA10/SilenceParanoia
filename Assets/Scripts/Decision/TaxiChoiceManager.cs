using UnityEngine;
using StarterAssets;
using UnityEngine.Playables;

public class TaxiChoiceManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject drinkWaterPanel;
    public GameObject stopCarPanel;

    [Header("Water Bottle In Hand")]
    public GameObject waterBottleInHand;

    [Header("Choice Sound")]
    public AudioSource choiceAudioSource;
    public AudioClip choiceLoopSound;

    TaxiDialogueManager dialogueManager;
    FirstPersonController firstPersonController;
    public Animator playerAnimator;
    public DrinkSequenceManager drinkSequenceManager;
    public CarLookController carLookController;
    public PlayableDirector timeline;

    [Header("Cameras")]
    public GameObject playerCamera;
    public GameObject timelineCamera;

    void Start()
    {
        if (drinkWaterPanel != null)
            drinkWaterPanel.SetActive(false);

        if (stopCarPanel != null)
            stopCarPanel.SetActive(false);

        if (waterBottleInHand != null)
            waterBottleInHand.SetActive(false);

        dialogueManager = FindObjectOfType<TaxiDialogueManager>();
        firstPersonController = FindAnyObjectByType<FirstPersonController>();
    }

    public void ShowWaterChoice()
    {
        if (drinkWaterPanel != null)
            drinkWaterPanel.SetActive(true);

        PlayChoiceSound();

        LockPlayer();
    }

    public void ChooseDrinkWater()
    {
        StopChoiceSound();

        if (drinkSequenceManager != null)
            drinkSequenceManager.StartDrinkSequence();

        // 🔁 สลับกล้อง
        if (playerCamera != null) playerCamera.SetActive(false);
        if (timelineCamera != null) timelineCamera.SetActive(true);

        // ▶️ เล่น Timeline
        timeline.time = 0;
        timeline.Play();

        CloseAllChoices();
    }

    public void ChooseRefuseWater()
    {
        StopChoiceSound();

        CloseAllChoices();
        dialogueManager?.StartRefusePhase();
    }

    public void ShowStopCarChoice()
    {
        if (stopCarPanel != null)
            stopCarPanel.SetActive(true);

        PlayChoiceSound();

        LockPlayer();
    }

    public void ChooseStopCar()
    {
        StopChoiceSound();

        CloseAllChoices();
        dialogueManager?.StartStopCarPhase();
    }

    public void ChooseNotStopCar()
    {
        StopChoiceSound();

        dialogueManager?.StartNoStopPhase();
        CloseAllChoices();
    }

    void CloseAllChoices()
    {
        if (drinkWaterPanel != null)
            drinkWaterPanel.SetActive(false);

        if (stopCarPanel != null)
            stopCarPanel.SetActive(false);

        UnlockPlayer();
    }

    void LockPlayer()
    {
        if (firstPersonController != null)
            firstPersonController.enabled = false;

        if (carLookController != null)
            carLookController.lockLook = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UnlockPlayer()
    {
        if (firstPersonController != null)
            firstPersonController.enabled = true;

        if (carLookController != null)
            carLookController.lockLook = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PlayChoiceSound()
    {
        if (choiceAudioSource != null && choiceLoopSound != null)
        {
            choiceAudioSource.clip = choiceLoopSound;
            choiceAudioSource.loop = true;
            choiceAudioSource.Play();
        }
    }

    void StopChoiceSound()
    {
        if (choiceAudioSource != null)
        {
            choiceAudioSource.Stop();
        }
    }
}