using UnityEngine;
using System.Collections;

public class SkillSequencer : MonoBehaviour
{
    [Header("Hitboxes")]
    public GameObject phase1Hitbox; // Kéo Hitbox_Phase1_Spin vào đây
    public GameObject phase2Hitbox; // Kéo Hitbox_Phase2_Bomb vào đây

    [Header("Timing")]
    public float phase1Duration = 1.5f; // Thời gian xoay bao lâu?
    public float phase2Duration = 0.5f; // Thời gian nổ tồn tại bao lâu?

    void Start()
    {
        // Bắt đầu chuỗi chiêu thức
        StartCoroutine(RunSkillSequence());
    }

    IEnumerator RunSkillSequence()
    {
        // --- GIAI ĐOẠN 1: XOAY ---
        phase1Hitbox.SetActive(true);
        phase2Hitbox.SetActive(false);
        
        // Chờ cho xoay xong (ví dụ 1.5 giây)
        yield return new WaitForSeconds(phase1Duration);

        // --- GIAI ĐOẠN 2: NỔ BOMB ---
        phase1Hitbox.SetActive(false); // Tắt hitbox xoay đi (để không gây dame nữa)
        phase2Hitbox.SetActive(true);  // Bật hitbox nổ lên -> Gây dame lần 2

        // Chờ cho nổ xong hiệu ứng
        yield return new WaitForSeconds(phase2Duration);

        // --- KẾT THÚC ---
        // Hủy toàn bộ skill này đi cho đỡ nặng game
        Destroy(gameObject);
    }
}