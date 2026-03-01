using UnityEngine;
using UnityEngine.SceneManagement; // 1. Cần thư viện này để check Scene
using System.Collections.Generic;

public class GameCursorManager : MonoBehaviour
{
    public static GameCursorManager Instance;

    [Header("CÀI ĐẶT SCENE")]
    // Bạn nhập tên các Scene chơi game vào đây (Ví dụ: "GameScene", "Level1")
    // Các Scene KHÔNG có trong list này sẽ mặc định HIỆN CHUỘT
    public List<string> gameplayScenes = new List<string>();

    [Header("TRẠNG THÁI CÁC UI (Tự động cập nhật)")]
    public bool isInventoryOpen = false;
    public bool isStoryUIOpen = false;
    public bool isPauseMenuOpen = false;

    void Awake()
    {
        // 2. Setup DontDestroyOnLoad chuẩn
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy bản sao nếu lỡ tạo thêm
        }
    }

    // 3. Đăng ký sự kiện khi chuyển Scene để reset trạng thái
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này tự chạy mỗi khi Load Scene mới
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset lại tất cả cờ UI về false để tránh lỗi kẹt trạng thái
        isInventoryOpen = false;
        isStoryUIOpen = false;
        isPauseMenuOpen = false;

        // Kiểm tra ngay khi vào Scene mới
        CheckCursorForCurrentScene();
    }

    void Update()
    {
        CheckCursorForCurrentScene();
    }

    void CheckCursorForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 4. LOGIC QUAN TRỌNG:
        // Nếu Scene hiện tại nằm trong danh sách "Gameplay Scenes" thì mới chạy logic ẩn/hiện
        if (gameplayScenes.Contains(currentSceneName))
        {
            // --- LOGIC CŨ (Trong màn chơi) ---
            bool isAnyUIOpen = isInventoryOpen || isStoryUIOpen || isPauseMenuOpen;

            if (isAnyUIOpen)
            {
                ShowCursor();
            }
            else
            {
                // Nếu không có UI -> Click chuột thì ẩn
                if (Input.GetMouseButtonDown(0))
                {
                    HideCursor();
                }
                
                // (Tùy chọn) Force ẩn luôn nếu đang không lock
                // if (Cursor.lockState == CursorLockMode.None && !isAnyUIOpen) HideCursor();
            }
        }
        else
        {
            // --- LOGIC MỚI (Menu, Lose, Win...) ---
            // Nếu không phải màn chơi -> LUÔN LUÔN HIỆN CHUỘT
            ShowCursor();
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