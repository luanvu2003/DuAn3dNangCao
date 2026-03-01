using UnityEngine;

// Dòng này giúp bạn chuột phải tạo file được trong Unity
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string itemName = "Tên Vật Phẩm";
    public Sprite icon; // Hình ảnh hiển thị trong túi
    
    [TextArea]
    public string description = "Mô tả công dụng của vật phẩm...";

    [Header("Loại vật phẩm")]
    public bool isStackable = true; // Có xếp chồng được không? (Ví dụ: Máu thì true, Kiếm thì false)
}