using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    public GameObject inventoryUI;
    public Transform slotContainer;
    public GameObject slotPrefab;
    public GameObject contextMenu; // Panel menu chuột phải

    [Header("Data")]
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private InventoryItem selectedItem;
    private bool isInventoryOpen = false;

    void Awake() { Instance = this; }

    void Start()
    {
        inventoryUI.SetActive(false);
        if (contextMenu != null) contextMenu.SetActive(false);
    }

    void Update()
    {
        // Bật tắt túi bằng phím I
        if (Input.GetKeyDown(KeyCode.I)) ToggleInventory();

        // Xử lý ẩn Context Menu khi click ra ngoài
        if (Input.GetMouseButtonDown(0) && contextMenu != null && contextMenu.activeSelf)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                contextMenu.SetActive(false);
            }
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (!isInventoryOpen && contextMenu != null)
            contextMenu.SetActive(false);

        if (isInventoryOpen) UpdateUI();

        // 🔥 BÁO CÁO TRẠNG THÁI CHO GAME MANAGER
        if (GameCursorManager.Instance != null)
            GameCursorManager.Instance.isInventoryOpen = isInventoryOpen;
    }

    // --- HÀM DÙNG ITEM (USE) ---
    public void OnUseButton()
    {
        if (selectedItem == null) return;

        // Kiểm tra loại item
        if (selectedItem.data.itemType == ItemType.Consumable)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterBaseStats stats = player.GetComponent<CharacterBaseStats>();
                if (stats != null)
                {
                    stats.Heal(selectedItem.data.effectAmount);
                }
            }
        }

        // Trừ số lượng
        selectedItem.stackSize--;
        if (selectedItem.stackSize <= 0) inventory.Remove(selectedItem);

        if (contextMenu != null) contextMenu.SetActive(false);
        UpdateUI();
    }

    // --- CÁC HÀM CƠ BẢN KHÁC ---
    public void AddItem(ItemData newItem)
    {
        if (newItem.isStackable)
        {
            foreach (InventoryItem item in inventory)
            {
                if (item.data == newItem) { item.AddToStack(); if (isInventoryOpen) UpdateUI(); return; }
            }
        }
        inventory.Add(new InventoryItem(newItem));
        if (isInventoryOpen) UpdateUI();
    }

    public void OpenContextMenu(InventoryItem item)
    {
        if (contextMenu == null) return;
        selectedItem = item;
        contextMenu.SetActive(true);
        contextMenu.transform.position = Input.mousePosition; // Đơn giản hóa vị trí
        contextMenu.transform.SetAsLastSibling();
    }

    public void OnRemoveButton()
    {
        if (selectedItem == null) return;
        inventory.Remove(selectedItem);
        UpdateUI();
        if (contextMenu != null) contextMenu.SetActive(false);
    }

    void UpdateUI()
    {
        foreach (Transform child in slotContainer) Destroy(child.gameObject);
        foreach (InventoryItem item in inventory)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            newSlot.GetComponent<InventorySlot>()?.SetItem(item);
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _data) { data = _data; stackSize = 1; }
    public void AddToStack() { stackSize++; }
}