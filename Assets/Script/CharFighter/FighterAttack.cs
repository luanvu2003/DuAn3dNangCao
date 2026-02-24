using UnityEngine;

public class FighterAttack : MonoBehaviour
{
    [Header("Cài đặt Combo")]
    public float attackCooldown = 0.4f; // Thời gian nghỉ giữa các cú đấm
    public float comboResetTime = 1.5f; // Thời gian reset combo về 1

    [Header("Cài đặt Hitbox (Quan trọng)")]
    public Transform attackPoint;    // Kéo cái object rỗng nằm trước mặt vào đây
    public float attackRange = 1.5f; // Bán kính quét
    public LayerMask enemyLayer;     // Chọn Layer là Enemy
    
    [Header("Chỉ số")]
    public float damage = 20f;
    public float rageGain = 15f;     // Hồi nộ khi đánh trúng

    [Header("Hiệu ứng (VFX)")]
    public GameObject hitEffectPrefab; // Kéo Prefab máu/lửa vào đây

    [Header("Components")]
    public Animator anim;

    // Biến nội bộ
    private float nextAttackTime = 0f;
    private float lastAttackTime = 0f;
    private int comboStep = 0; 

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        // Nếu quên gán AttackPoint thì lấy tạm vị trí của nhân vật
        if (attackPoint == null) attackPoint = transform;
    }

    void Update()
    {
        // 1. Đang giữ E (Gồng skill) thì không đánh thường
        if (Input.GetKey(KeyCode.E)) return;

        // 2. Tự reset combo nếu nghỉ quá lâu
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        // 3. Click chuột để đánh
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
        lastAttackTime = Time.time;
        nextAttackTime = Time.time + attackCooldown;

        // Logic Combo 1-2
        if (comboStep == 0)
        {
            anim.SetTrigger("FighterAttack1");
            comboStep = 1;
        }
        else 
        {
            anim.SetTrigger("FighterAttack2");
            comboStep = 0;
        }
    }

    // --- HÀM NÀY SẼ GẮN VÀO ANIMATION EVENT ---
    // (Nó thay thế hoàn toàn cho cái Collider ở tay)
    public void OnAttackHit()
    {
        // 1. Quét xung quanh điểm AttackPoint xem có quái nào không
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        // 2. Duyệt qua từng con quái trúng đòn
        foreach (Collider enemy in hitEnemies)
        {
            // --- GÂY DAME ---
            // Ưu tiên tìm script CharacterBaseStats (Hệ thống mới)
            CharacterBaseStats stats = enemy.GetComponent<CharacterBaseStats>();
            
            // Nếu không thấy thì tìm EnemyHealth (Hệ thống cũ)
            EnemyHealth legacyHealth = null;
            if (stats == null) 
            {
                stats = enemy.GetComponentInParent<CharacterBaseStats>();
                if(stats == null) legacyHealth = enemy.GetComponent<EnemyHealth>();
            }

            if (stats != null) stats.TakeDamage(damage);
            else if (legacyHealth != null) legacyHealth.TakeDamage((int)damage);

            // --- HỒI NỘ ---
            if (FighterSkills.instance != null)
            {
                FighterSkills.instance.AddRage(rageGain);
            }

            // --- HIỆN EFFECT (PARTICLE) ---
            if (hitEffectPrefab != null)
            {
                // Tìm điểm va chạm gần nhất trên người quái để effect nổ ngay da thịt
                Vector3 hitPos = enemy.ClosestPoint(attackPoint.position);
                
                GameObject vfx = Instantiate(hitEffectPrefab, hitPos, Quaternion.identity);
                Destroy(vfx, 1f); // Xóa effect sau 1 giây
            }

            Pillar pillar = enemy.GetComponent<Pillar>();
            if (pillar != null)
            {
                pillar.TakeDamage(damage);
            }
        }
    }

    // Vẽ vòng tròn đỏ trong Scene để dễ chỉnh tầm đánh
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}