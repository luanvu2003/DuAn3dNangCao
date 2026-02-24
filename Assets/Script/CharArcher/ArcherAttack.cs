using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    [Header("Settings")]
    public float attackCooldown = 0.5f; 
    

    [Header("Components")]
    public Animator anim;

    private float nextAttackTime = 0f;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
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

        // 3. Set thời gian hồi chiêu
        nextAttackTime = Time.time + attackCooldown;
    }
}