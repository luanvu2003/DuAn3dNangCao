using UnityEngine;

// Dòng này cực quan trọng: Nó bắt script này chạy sau cùng,
// đè lên mọi logic của BasicController.
[DefaultExecutionOrder(9999)] 
public class MagicMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeedLimit = 2.0f; // Tốc độ giới hạn khi đi bộ
    public float runThreshold = 0.1f;

    [Header("Visual Settings")]
    public float walkAnimSpeed = 0.5f;  // Tua chậm animation

    [Header("Components")]
    public Rigidbody rb;
    public Animator anim;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    // Chuyển sang FixedUpdate để xử lý vật lý chuẩn hơn
    void FixedUpdate()
    {
        bool isHoldingCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Lấy vận tốc hiện tại (Unity 6 dùng linearVelocity)
        Vector3 currentVelocity = rb.linearVelocity;
        
        // Tính vận tốc mặt phẳng (bỏ qua trục Y)
        Vector3 flatVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        float currentSpeed = flatVelocity.magnitude;
        anim.SetBool("IsMoving", currentSpeed > 0.1f);

        // --- BƯỚC 1: XỬ LÝ VẬT LÝ (QUAN TRỌNG NHẤT) ---
        if (isHoldingCtrl)
        {
            // Nếu tốc độ hiện tại đang lớn hơn giới hạn đi bộ
            if (currentSpeed > walkSpeedLimit)
            {
                // Ép vận tốc xuống đúng bằng walkSpeedLimit
                Vector3 clampedVelocity = flatVelocity.normalized * walkSpeedLimit;
                
                // Gán ĐÈ NGƯỢC LẠI vào Rigidbody ngay lập tức
                rb.linearVelocity = new Vector3(clampedVelocity.x, currentVelocity.y, clampedVelocity.z);
            }
        }
    }

    void Update() // Xử lý Animation ở Update cho mượt
    {
        bool isHoldingCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        
        // Logic Animation
        // (Lưu ý: Nếu BasicController cũng set animation thì có thể sẽ lại bị đánh nhau ở đây)
        anim.SetBool("IsWalking", isHoldingCtrl);

        if (isHoldingCtrl)
        {
            anim.speed = walkAnimSpeed;
        }
        else
        {
            anim.speed = 1f;
        }
    }
}