using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] string itemName = "Bottle";

    public string Prompt => "Pick Up" + itemName;

    public void Interact()
    {
        Destroy(gameObject);
    }
}
