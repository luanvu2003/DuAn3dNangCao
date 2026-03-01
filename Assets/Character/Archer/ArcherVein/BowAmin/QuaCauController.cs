using UnityEngine;

public class QuaCauController : MonoBehaviour
{
    public GameObject quacauObject;           
    public ParticleSystem disappearEffect; 
    public GameObject arrowPrefab;         
    public Transform shootPoint;           
    public float arrowForce = 20f;         
    public float arrowDelay = 0.5f;

    [Header("Cooldown Settings")] // [MỚI] Cài đặt hồi chiêu
    public float attackCooldown = 1.0f; // Thời gian hồi chiêu (1 giây)
    private float lastAttackTime = -Mathf.Infinity; // Thời điểm bắn lần cuối (âm vô cùng để bắn được ngay lần đầu)

    private bool isPlaying = false;
    private float animationLength = 0f;

    void Start()
    {

    }

    void Update()
    {
        // Nếu đang giữ E thì BowController này không tự xử lý chuột trái nữa
        if (Input.GetKey(KeyCode.E)) return; 

        // [MỚI] Thêm điều kiện check thời gian hồi chiêu
        // Time.time >= lastAttackTime + attackCooldown: Nghĩa là thời gian hiện tại đã vượt qua thời điểm được phép bắn tiếp theo chưa
        if (Input.GetMouseButtonDown(0) && !isPlaying && Time.time >= lastAttackTime + attackCooldown)
        {
            // [MỚI] Cập nhật thời điểm bắn mới nhất
            lastAttackTime = Time.time;

            ShowQuaCauAndPlayAnim(); 
            
            // Bắn thường
            Invoke(nameof(ShootArrowBasic), arrowDelay);
            Invoke(nameof(HideQuaCau), animationLength);
        }
    }

    // --- CÁC HÀM PUBLIC ĐỂ SKILL E GỌI ---

    public void ShowQuaCauAndPlayAnim()
    {
        quacauObject.SetActive(true);
        isPlaying = true;
    }

    public void HideQuaCau()
    {
        if (!quacauObject.activeSelf) return; 

        if (disappearEffect != null)
        {
            disappearEffect.transform.position = quacauObject.transform.position;
            disappearEffect.Play();
        }

        quacauObject.SetActive(false);
        isPlaying = false;
    }

    // Hàm bắn thường (Private)
    void ShootArrowBasic()
    {
        if (arrowPrefab != null && shootPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            arrow.transform.Rotate(0, -90, 0); 
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(shootPoint.forward * arrowForce, ForceMode.Impulse);
            Destroy(arrow, 3f);
        }
    }
}