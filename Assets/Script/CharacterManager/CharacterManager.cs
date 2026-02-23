using UnityEngine;
public class CharacterManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject[] characters; // Kéo 3 nhân vật vào đây
    public vThirdPersonCamera cameraScript; // Kéo cái Main Camera vào đây

    private int currentIndex = 0; // Mặc định là 0

    void Start()
    {
        // 1. Tìm script camera nếu quên kéo
        if (cameraScript == null) 
            cameraScript = FindFirstObjectByType<vThirdPersonCamera>();

        // 2. Setup trạng thái ban đầu
        // Tắt hết các nhân vật khác, chỉ để lại con đầu tiên
        for (int i = 0; i < characters.Length; i++)
        {
            if (i == 0)
            {
                characters[i].SetActive(true);
                // Ép camera nhìn vào nhân vật 0 ngay từ đầu
                if (cameraScript != null) cameraScript.SetMainTarget(characters[i].transform);
            }
            else
            {
                characters[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Swap(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Swap(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Swap(2);
    }

    public void Swap(int newIndex)
    {
        // Nếu chọn đúng con đang chơi thì thôi
        if (newIndex == currentIndex) return;
        if (newIndex >= characters.Length) return;

        // --- BƯỚC 1: LẤY VỊ TRÍ CỦA NHÂN VẬT CŨ ---
        // Để nhân vật mới xuất hiện đúng chỗ nhân vật cũ (giống Genshin)
        Transform oldChar = characters[currentIndex].transform;
        Vector3 pos = oldChar.position;
        Quaternion rot = oldChar.rotation;

        // Tắt nhân vật cũ
        oldChar.gameObject.SetActive(false);

        // --- BƯỚC 2: BẬT NHÂN VẬT MỚI ---
        GameObject newChar = characters[newIndex];
        
        // Cập nhật vị trí cho nhân vật mới trước khi bật lên
        newChar.transform.position = pos;
        newChar.transform.rotation = rot;
        
        newChar.SetActive(true);

        // --- BƯỚC 3: CẬP NHẬT CAMERA (CHÌA KHÓA CỦA VẤN ĐỀ) ---
        // Gọi hàm SetMainTarget để Camera nhận diện lại mục tiêu mới
        if (cameraScript != null)
        {
            cameraScript.SetMainTarget(newChar.transform);
        }

        // Lưu lại index
        currentIndex = newIndex;
    }
}
