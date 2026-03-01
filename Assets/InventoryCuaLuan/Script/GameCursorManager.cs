using UnityEngine;

public class GameCursorManager : MonoBehaviour
{
    public static GameCursorManager Instance;

    [Header("TRẠNG THÁI CÁC UI (Tự động cập nhật)")]
    public bool isInventoryOpen = false;
    public bool isStoryUIOpen = false;   // Hội thoại / Quest Panel
    public bool isPauseMenuOpen = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Mặc định vào game là ẩn chuột
        HideCursor();
    }

    void Update()
    {
        // 1. Kiểm tra xem có bất kỳ UI nào đang mở không?
        bool isAnyUIOpen = isInventoryOpen || isStoryUIOpen || isPauseMenuOpen;

        if (isAnyUIOpen)
        {
            // Nếu có UI mở -> BẮT BUỘC HIỆN CHUỘT
            ShowCursor();
        }
        else
        {
            // Nếu không có UI nào mở -> Click vào màn hình thì ẩn chuột (để xoay camera)
            if (Input.GetMouseButtonDown(0))
            {
                HideCursor();
            }
        }
    }

    public void ShowCursor()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HideCursor()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}