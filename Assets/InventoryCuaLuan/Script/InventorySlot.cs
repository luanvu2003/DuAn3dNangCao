using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // 1. BẮT BUỘC PHẢI CÓ DÒNG NÀY

// 2. Thêm IPointerClickHandler để bắt sự kiện click chuột
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Thành phần UI")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    private InventoryItem currentItem; // Biến lưu món đồ hiện tại của ô này

    // 3. SỬA HÀM NÀY: Nhận vào InventoryItem thay vì ItemData
    public void SetItem(InventoryItem item)
    {
        currentItem = item; // Lưu lại để tí nữa biết đang click vào món nào

        // Cập nhật UI
        if (item != null && item.data != null)
        {
            iconImage.sprite = item.data.icon;
            iconImage.enabled = true;

            if (nameText != null)
                nameText.text = item.data.itemName;

            // Nếu số lượng > 1 thì hiện số, không thì để trống
            if (amountText != null)
                amountText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }

    // 4. Hàm này tự động chạy khi click chuột vào ô
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("1. Đã click chuột phải!");

            // Kiểm tra từng nguyên nhân
            if (currentItem == null)
            {
                Debug.LogError("LỖI: currentItem đang bị NULL! (Bạn chưa gán currentItem = item trong hàm SetItem)");
                return;
            }

            if (InventoryManager.Instance == null)
            {
                Debug.LogError("LỖI: InventoryManager chưa chạy hoặc không tìm thấy Instance!");
                return;
            }

            // Nếu qua được 2 cửa ải trên thì mới chạy dòng này
            Debug.Log("2. Mọi thứ OK -> Gửi lệnh mở Menu!");
            InventoryManager.Instance.OpenContextMenu(currentItem);
        }
    }
}
