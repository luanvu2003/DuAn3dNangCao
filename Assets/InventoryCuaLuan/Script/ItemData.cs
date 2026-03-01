using UnityEngine;

public enum ItemType 
{ 
    Resource,   // Nguyên liệu
    Consumable, // Đồ tiêu thụ (Máu, Mana...)
    Equipment   // Trang bị
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable;

    [Header("Item Effect")]
    public ItemType itemType; // Chọn loại item là Consumable
    public int effectAmount;  // Hồi bao nhiêu? (Ví dụ: 50)
}