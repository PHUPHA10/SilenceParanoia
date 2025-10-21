using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Definition", fileName = "NewItem")]
public class ItemDefinition : ScriptableObject
{
    public string displayName;
    public Sprite icon;               // ไม่ได้โชว์ แต่เก็บไว้ใช้ภายหลัง
    public int maxStack = 1;
    [Header("Optional - show in hand")]
    public GameObject heldPrefab;     // ถ้าอยากให้ถือ
    public Vector3 heldLocalPosition = new Vector3(0.38f, -0.38f, 0.72f);
    public Vector3 heldLocalEuler    = new Vector3(8f, 0f, 0f);
    public Vector3 heldLocalScale    = Vector3.one;

}