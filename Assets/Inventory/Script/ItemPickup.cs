using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickup : MonoBehaviour
{
    public ItemRT item;
    void Pickup()
    {
        Destroy(this.gameObject);
        InventoryManager.Instance.Add(item);
    }
    void OnMouseDown()
    {
        Pickup();
    }
}
