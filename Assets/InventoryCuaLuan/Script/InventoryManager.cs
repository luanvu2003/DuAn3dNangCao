using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    public GameObject inventoryUI;
    public Transform slotContainer;
    public GameObject slotPrefab;

    [Header("Context Menu")] // [MỚI] Kéo cái bảng ContextMenu (Panel nhỏ) vào đây
    public GameObject contextMenu;

    [Header("Data")]
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private InventoryItem selectedItem; // Món đồ đang được chọn để xử lý
    private bool isInventoryOpen = false;

    void Awake() { Instance = this; }

    void Start()
    {
        // Xóa sạch túi lúc đầu game (nếu muốn)
        // inventory.Clear(); 

        inventoryUI.SetActive(false);
        if (contextMenu != null) contextMenu.SetActive(false); // Đảm bảo menu ẩn lúc đầu
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) ToggleInventory();

        // --- ĐOẠN CODE SỬA LẠI ---
        if (Input.GetMouseButtonDown(0) && contextMenu != null && contextMenu.activeSelf)
        {
            // Kiểm tra: Con chuột có đang đè lên cái nút UI nào không?
            // Nếu KHÔNG đè lên UI (tức là click ra ngoài khoảng trống) thì mới tắt menu
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                contextMenu.SetActive(false);
            }
        }
    }

    // --------------------------------------------------
    // 🔔 PHẦN XỬ LÝ CONTEXT MENU (CLICK CHUỘT PHẢI)
    // --------------------------------------------------
    public void OpenContextMenu(InventoryItem item)
    {
        if (contextMenu == null) return;

        selectedItem = item;
        contextMenu.SetActive(true);

        // --- ĐOẠN CODE SỬA LẠI ---

        // Cách A: Nếu dùng Canvas Overlay (Code cũ)
        // contextMenu.transform.position = Input.mousePosition;

        // Cách B: Chuẩn nhất cho mọi loại Canvas (Overlay hay Camera đều chạy)
        // Lấy RectTransform của cha nó (thường là InventoryPanel hoặc Canvas)
        RectTransform parentRect = contextMenu.transform.parent.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            Input.mousePosition,
            null, // Nếu dùng Camera thì thay null bằng Camera.main, nhưng thường null là tự hiểu
            out localPoint
        );

        contextMenu.transform.localPosition = localPoint;
        // -------------------------

        // Đưa lên trên cùng để không bị che
        contextMenu.transform.SetAsLastSibling();
    }

    // Gán hàm này vào nút "Use" (Dùng) trên ContextMenu
    public void OnUseButton()
    {
        Debug.Log("==> Đã bấm nút USE!"); // 1. Kiểm tra xem nút có ăn không?

        if (selectedItem == null)
        {
            Debug.LogError("LỖI: selectedItem đang bị Null! (Có thể do chưa lưu item khi click chuột phải)");
            return;
        }

        Debug.Log("Đang xử lý vật phẩm: " + selectedItem.data.itemName + " | Số lượng cũ: " + selectedItem.stackSize);

        // Trừ số lượng
        selectedItem.stackSize--;

        Debug.Log("Số lượng mới: " + selectedItem.stackSize);

        // Nếu hết thì xóa
        if (selectedItem.stackSize <= 0)
        {
            Debug.Log("Hết hàng -> Xóa khỏi list!");
            inventory.Remove(selectedItem);
        }

        // Tắt menu
        if (contextMenu != null) contextMenu.SetActive(false);

        // QUAN TRỌNG: Vẽ lại UI
        Debug.Log("Đang gọi UpdateUI()...");
        UpdateUI();
    }

    // --------------------------------------------------
    // XỬ LÝ KHI ẤN NÚT "VỨT" (REMOVE)
    // --------------------------------------------------
    public void OnRemoveButton()
    {
        if (selectedItem == null) return;

        Debug.Log($"Đã vứt bỏ: {selectedItem.data.itemName}");

        // Xóa thẳng khỏi list luôn, không cần trừ số lượng
        inventory.Remove(selectedItem);

        // Vẽ lại túi ngay lập tức
        UpdateUI();

        if (contextMenu != null) contextMenu.SetActive(false);
        selectedItem = null;
    }

    // --------------------------------------------------
    // CÁC HÀM CŨ (ADD ITEM, TOGGLE...)
    // --------------------------------------------------
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (!isInventoryOpen && contextMenu != null)
            contextMenu.SetActive(false); // Đóng túi thì tắt luôn menu

        if (isInventoryOpen)
        {
            UpdateUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddItem(ItemData newItem)
    {
        if (newItem.isStackable)
        {
            foreach (InventoryItem item in inventory)
            {
                if (item.data == newItem)
                {
                    item.AddToStack();
                    if (isInventoryOpen) UpdateUI();
                    return;
                }
            }
        }
        InventoryItem newSlot = new InventoryItem(newItem);
        inventory.Add(newSlot);
        if (isInventoryOpen) UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject); // Xóa sạch ô cũ trước khi vẽ ô mới
        }

        foreach (InventoryItem item in inventory)
        {
            GameObject newSlotObj = Instantiate(slotPrefab, slotContainer);
            InventorySlot slotScript = newSlotObj.GetComponent<InventorySlot>();

            if (slotScript != null)
            {
                // Gọi hàm SetItem mới (truyền InventoryItem)
                slotScript.SetItem(item);
            }
        }
    }
}

// Class này nằm cùng file InventoryManager nhưng ở ngoài class chính
[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _data)
    {
        data = _data;
        stackSize = 1;
    }

    public void AddToStack()
    {
        stackSize++;
    }
}