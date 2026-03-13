using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public CharacterSwitcher characterSwitcher;

    private CharacterNameButton currentSelected;

    public void Select(CharacterNameButton newSelection)
    {
        if (currentSelected != null)
            currentSelected.SetNormal();

        currentSelected = newSelection;
        currentSelected.SetSelected();

        // สลับตัวละครตาม ID
        characterSwitcher.ShowCharacter(newSelection.characterID);
    }
}