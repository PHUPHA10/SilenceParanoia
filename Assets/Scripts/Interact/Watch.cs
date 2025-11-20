using UnityEngine;

public class Watch : MonoBehaviour
{
    [Header("Watch Info")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData;   // ?????? ScriptableObject ?????????
    [SerializeField] private int amount = 0;

    public string Prompt =>

        " Check outside the room " + (string.IsNullOrEmpty(itemName) ?
        itemData?.displayName :
        itemName);


    public void Interact()
    {

    }
}
