using UnityEngine;
using UnityEngine.SceneManagement; // Dùng để chuyển scene Lose

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance; // Tạo Singleton để các con gọi về

    [Header("Setup")]
    public GameObject[] characters; // Kéo 3 nhân vật vào đây
    public vThirdPersonCamera cameraScript; // Kéo Main Camera vào

    [Header("Debug Info")]
    public int currentIndex = 0;
    
    // Mảng lưu chỉ số Stats để kiểm tra máu/chết
    private CharacterBaseStats[] charStats;

    void Awake()
    {
        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 1. Tự tìm Camera nếu quên kéo
        if (cameraScript == null) 
            cameraScript = FindFirstObjectByType<vThirdPersonCamera>();

        // 2. Lấy script Stats của các nhân vật để check chết sau này
        charStats = new CharacterBaseStats[characters.Length];
        for(int i=0; i<characters.Length; i++)
        {
            charStats[i] = characters[i].GetComponent<CharacterBaseStats>();
        }

        // 3. Setup trạng thái ban đầu (Chỉ bật con đầu tiên)
        InitCharacters();
    }

    void InitCharacters()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            bool isActive = (i == currentIndex);
            characters[i].SetActive(isActive);

            if (isActive && cameraScript != null)
                cameraScript.SetMainTarget(characters[i].transform);
        }
    }

    void Update()
    {
        // Bấm phím để đổi nhân vật
        if (Input.GetKeyDown(KeyCode.Alpha1)) RequestSwap(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) RequestSwap(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) RequestSwap(2);
    }

    // --- HÀM KIỂM TRA ĐIỀU KIỆN TRƯỚC KHI ĐỔI ---
    public void RequestSwap(int newIndex)
    {
        // 1. Kiểm tra index hợp lệ
        if (newIndex < 0 || newIndex >= characters.Length) return;
        
        // 2. Không đổi sang chính mình
        if (newIndex == currentIndex) return;

        // 3. 🔥 KIỂM TRA CHẾT: Nếu con định đổi sang đã chết -> Không cho đổi
        if (charStats[newIndex] != null && charStats[newIndex].isDead)
        {
            Debug.Log("Nhân vật " + characters[newIndex].name + " đã chết, không thể đổi!");
            return; 
        }

        // Nếu ok hết -> Thực hiện đổi
        PerformSwap(newIndex);
    }

    // --- HÀM THỰC HIỆN ĐỔI (Logic cũ của bạn + PartyManager) ---
    private void PerformSwap(int newIndex)
    {
        // A. LẤY VỊ TRÍ CỦA CON CŨ
        Transform oldChar = characters[currentIndex].transform;
        Vector3 pos = oldChar.position;
        Quaternion rot = oldChar.rotation;

        // Tắt con cũ
        oldChar.gameObject.SetActive(false);

        // B. BẬT CON MỚI
        GameObject newChar = characters[newIndex];
        
        // Đồng bộ vị trí
        newChar.transform.position = pos;
        newChar.transform.rotation = rot;
        
        newChar.SetActive(true);

        // C. CẬP NHẬT CAMERA
        if (cameraScript != null)
        {
            cameraScript.SetMainTarget(newChar.transform);
        }

        // Lưu lại index
        currentIndex = newIndex;
        Debug.Log("Đã đổi sang: " + newChar.name);
    }

    // --- HÀM XỬ LÝ KHI CÓ NHÂN VẬT CHẾT (Gọi từ CharacterBaseStats) ---
    public void OnCharacterDied(CharacterBaseStats deadChar)
    {
        // 1. Tìm xem còn ai sống không
        int nextAliveIndex = -1;

        // Duyệt qua danh sách để tìm người sống (ưu tiên người kế tiếp)
        for (int i = 0; i < characters.Length; i++)
        {
            // Bỏ qua người vừa chết
            if (charStats[i] == deadChar) continue;

            // Nếu tìm thấy người chưa chết
            if (!charStats[i].isDead)
            {
                nextAliveIndex = i;
                break;
            }
        }

        if (nextAliveIndex != -1)
        {
            // Còn người sống -> Tự động đổi sang
            Debug.Log("Tự động đổi sang người còn sống: " + characters[nextAliveIndex].name);
            PerformSwap(nextAliveIndex);
        }
        else
        {
            // Tất cả đều chết -> GAME OVER
            Debug.Log("Tất cả đã chết -> LOSE GAME");
            Invoke("LoadLoseScene", 2f); // Đợi 2s rồi chuyển cảnh
        }
    }

    void LoadLoseScene()
    {
        SceneManager.LoadScene("youlose"); // Nhớ đổi tên Scene cho đúng
    }
}