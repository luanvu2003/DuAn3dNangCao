using UnityEngine;
using static UnityEditor.Progress;

public class ItemUIController : MonoBehaviour
{
    public ItemRT item;
    public void SetItem(ItemRT item)
    {
        this.item = item;
    }
    public void Remove()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(this.gameObject);
    }
    public void UseItem()
    {
        switch (item.itemType)
        {
            case ItemType.Consumables:
                Debug.Log("Sử dụng vật phẩm hồi máu: " + item.itemName);
                break;

            case ItemType.Weapons:
                Debug.Log("Sử dụng vũ khí: " + item.itemName);
                break;

            default:
                break;
        }

        Remove();
    }


}



