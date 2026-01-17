using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    [Header("Settings")]
    public float attackCooldown = 0.5f; // Thời gian nghỉ giữa các lần đánh (giây)

    [Header("Components")]
    public Animator anim;

    private float nextAttackTime = 0f;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Kiểm tra thời gian hồi chiêu
        if (Time.time >= nextAttackTime)
        {
            // Kiểm tra click chuột trái (0 là trái, 1 là phải, 2 là giữa)
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        // 1. Kích hoạt animation tấn công
        // Bạn cần tạo Trigger tên là "Attack" trong Animator
        anim.SetTrigger("Attack");

        // 2. Set thời gian hồi chiêu cho lần đánh tiếp theo
        nextAttackTime = Time.time + attackCooldown;

        // (Sau này sẽ thêm code gây sát thương ở đây)
        Debug.Log("Đang tấn công!"); 
    }
}