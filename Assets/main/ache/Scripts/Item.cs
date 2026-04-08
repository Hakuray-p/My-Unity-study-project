using UnityEngine;

public enum ItemType
{
    Material,
    Potion
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Alchemy/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public string description;
    public Sprite icon; // 可选，用于 UI
}
