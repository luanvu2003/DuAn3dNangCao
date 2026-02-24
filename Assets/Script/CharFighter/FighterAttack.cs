using UnityEngine;

public class FighterAttack : MonoBehaviour
{
    [Header("Cài đặt Combo")]
    public float attackCooldown = 0.4f; // Thời gian nghỉ giữa các cú đấm (để không spam quá nhanh lỗi anim)
    public float comboResetTime = 1.5f; // Nếu nghỉ tay quá 1.5s, combo sẽ reset về đòn 1

    [Header("Components")]
    public Animator anim;

    // Biến nội bộ
    private float nextAttackTime = 0f;
    private float lastAttackTime = 0f;
    private int comboStep = 0; // 0 là đòn 1, 1 là đòn 2

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Nếu đang giữ E (Gồng Skill) thì KHÔNG được đánh thường
        if (Input.GetKey(KeyCode.E)) return;

        // 2. Logic Tự Reset Combo
        // Nếu khoảng cách từ lần đánh cuối đến giờ đã quá lâu -> Reset về đòn 1
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        // 3. Xử lý đánh thường
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PerformAttack();
            }
        }
    }

    void PerformAttack()
    {
        // Cập nhật thời gian
        lastAttackTime = Time.time;
        nextAttackTime = Time.time + attackCooldown;

        // --- LOGIC 1 - 2 - 1 - 2 ---
        if (comboStep == 0)
        {
            anim.SetTrigger("FighterAttack1"); // Chạy Anim đấm trái
            comboStep = 1;              // Chuẩn bị cho lần tới là đấm phải
        }
        else // comboStep == 1
        {
            anim.SetTrigger("FighterAttack2"); // Chạy Anim đấm phải
            comboStep = 0;              // Quay vòng về đấm trái
        }

        Debug.Log("Đánh thường: " + (comboStep == 0 ? "Đòn 2" : "Đòn 1"));
    }
}