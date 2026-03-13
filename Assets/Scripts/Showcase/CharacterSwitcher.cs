using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public GameObject characterObject;
    public string[] animations; // animation ของตัวนี้
}

public class CharacterSwitcher : MonoBehaviour
{
    public CharacterData[] characters;

    private GameObject currentCharacter;
    private Animator currentAnimator;
    private string[] currentAnimations;
    private int currentCharacterIndex = 0;
    private int animationIndex = 0;

    void Start()
    {
        ShowCharacter(0);
    }

    public void ShowCharacter(int index)
    {
        if (currentCharacter != null)
            currentCharacter.SetActive(false);

        currentCharacterIndex = index;

        currentCharacter = characters[index].characterObject;
        currentCharacter.SetActive(true);

        currentAnimator = currentCharacter.GetComponent<Animator>();
        currentAnimations = characters[index].animations;

        animationIndex = 0;
        PlayCurrentAnimation();
    }

    public void NextAnimation()
    {
        animationIndex++;
        if (animationIndex >= currentAnimations.Length)
            animationIndex = 0;

        PlayCurrentAnimation();
    }

    public void PreviousAnimation()
    {
        animationIndex--;
        if (animationIndex < 0)
            animationIndex = currentAnimations.Length - 1;

        PlayCurrentAnimation();
    }

    private void PlayCurrentAnimation()
    {
        if (currentAnimator != null && currentAnimations.Length > 0)
        {
            currentAnimator.CrossFade(currentAnimations[animationIndex], 0.2f);
        }
    }
}