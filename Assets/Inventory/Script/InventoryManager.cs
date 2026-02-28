using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform itemHolder;
    public GameObject itemPrefab;
    public Toggle enableRemoveButton;
    public static InventoryManager Instance { get; private set; }
    public List<ItemRT> items = new List<ItemRT>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Add(ItemRT item)
    {
        items.Add(item);
        DisplayInventory();
    }
    public void Remove(ItemRT item)
    {
        items.Remove(item);
    }

    public void DisplayInventory()
    {
        EnableRemoveButton();
        foreach (Transform child in itemHolder)
            Destroy(child.gameObject);

        foreach (ItemRT item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemHolder);
            TextMeshProUGUI itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            Image itemImage = obj.transform.Find("ItemImage").GetComponent<Image>();

            itemName.text = item.itemName;
            itemImage.sprite = item.image;
            obj.GetComponent<ItemUIController>().SetItem(item);
        }
    }
    public void EnableRemoveButton()
    {
        if (enableRemoveButton.isOn)
        {
            foreach (Transform item in itemHolder)
                item.transform.Find("RemoveButton")
                    .gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform item in itemHolder)
                item.transform.Find("RemoveButton")
                    .gameObject.SetActive(false);
        }
    }


}
