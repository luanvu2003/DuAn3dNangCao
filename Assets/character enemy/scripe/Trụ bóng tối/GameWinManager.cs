using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc để chuyển Scene

public class GameWinManager : MonoBehaviour
{
    public static GameWinManager Instance;

    [Header("Cài đặt chiến thắng")]
    public int targetPillars = 5; // Số trụ cần phá (5)
    public string winSceneName = "youwin"; // Tên Scene thắng (bạn đặt là "Win" hay "uiwn" thì điền vào đây)

    private int destroyedCount = 0; // Biến đếm số trụ đã phá

    void Awake()
    {
        // Singleton pattern để gọi từ bất cứ đâu
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Hàm này sẽ được gọi mỗi khi một trụ bị nổ
    public void OnPillarDestroyed()
    {
        destroyedCount++;
        Debug.Log("Tiến độ chiến thắng: " + destroyedCount + " / " + targetPillars);

        if (destroyedCount >= targetPillars)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Debug.Log(">>> CHIẾN THẮNG! Chuyển Scene...");

        // 1. Hiện lại con trỏ chuột (quan trọng để bấm nút ở màn hình Win)
        if (GameCursorManager.Instance != null)
        {
            GameCursorManager.Instance.ShowCursor();
        }

        // 2. Chuyển sang Scene Win
        SceneManager.LoadScene(winSceneName);
    }
}