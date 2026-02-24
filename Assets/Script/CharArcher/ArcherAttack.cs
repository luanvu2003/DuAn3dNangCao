using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    [Header("Settings")]
    public float attackCooldown = 0.5f; 
    
    // Nếu bạn muốn sinh mũi tên bay ra khi đánh thường thì kéo Prefab vào đây
    public GameObject arrowPrefab; 
    public Transform firePoint;

    [Header("Components")]
    public Animator anim;

    private float nextAttackTime = 0f;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        // QUAN TRỌNG: Nếu đang giữ nút E (đang ngắm Skill) thì KHÔNG được đánh thường
        if (Input.GetKey(KeyCode.E)) return;

        // Kiểm tra thời gian hồi chiêu
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        // 1. Kích hoạt animation (Trigger này sẽ nằm ở Layer UpperBody)
        if(anim) anim.SetTrigger("ArcherBasicAttack");

        // 2. Logic sinh mũi tên (nếu muốn bắn ra đạn thật)
        // Nên dùng Animation Event để gọi hàm này cho khớp tay, nhưng tạm thời gọi luôn cho nhạy
        if(arrowPrefab != null)
        {
            Instantiate(arrowPrefab, firePoint.position, transform.rotation);
        }

        // 3. Set thời gian hồi chiêu
        nextAttackTime = Time.time + attackCooldown;
    }
}