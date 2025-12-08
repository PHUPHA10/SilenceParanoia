using UnityEngine;

public class Hiding : MonoBehaviour
{
    [Header("Hide Info")]
    [SerializeField] private string itemName = "";
    [SerializeField] private ItemDefinition itemData; 
    [SerializeField] private int amount = 0;

    public string Prompt =>

        " ซ่อน " + (string.IsNullOrEmpty(itemName) ?
        itemData?.displayName :
        itemName);


    public void Interact()
    {

    }
}
