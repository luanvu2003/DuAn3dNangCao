using UnityEngine;

public class ArcherSkills : CharacterBaseStats 
{
    public static ArcherSkills instance;

    [Header("Skill E Settings")]
    public GameObject arrowPrefab; 
    public Transform firePoint;     
    public float cooldownE = 3f;

    [Header("Skill Q Settings (Ultimate)")]
    public float cooldownQ = 5f; 
    public float rageCostQ = 100f; 

    [Header("Components")]
    public Animator anim;

    private bool isAiming = false; 

    void OnEnable()
    {
        instance = this;
        isAiming = false; // Reset trạng thái để tránh lỗi anim
    }

    protected override void Start() 
    {
        base.Start(); 
        if (anim == null) anim = GetComponent<Animator>();
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        // --- LOGIC SKILL E ---
        // 1. GIỮ E ĐỂ NGẮM
        if (Input.GetKey(KeyCode.E))
        {
            // Chỉ cho ngắm nếu skill đã hồi xong
            if (IsSkillReady("Skill_E"))
            {
                if (!isAiming)
                {
                    isAiming = true;
                    // Bật trạng thái ngắm -> Animator sẽ chạy clip "Archer_AimLoop"
                    if(anim) anim.SetBool("IsAiming", true);
                }

                // 2. CLICK CHUỘT TRÁI ĐỂ BẮN (Khi đang giữ E)
                if (Input.GetMouseButtonDown(0))
                {
                    FireArrow();
                }
            }
        }

        // 3. THẢ E -> HỦY NGẮM (Nếu chưa bắn)
        if (Input.GetKeyUp(KeyCode.E))
        {
            isAiming = false;
            if(anim) anim.SetBool("IsAiming", false);
        }

        // --- LOGIC SKILL Q (Giữ nguyên) ---
        if (Input.GetKeyDown(KeyCode.Q) && IsSkillReady("Skill_Q"))
        {
            if (currentRage >= rageCostQ) UseSkillQ();
        }
    }

    void FireArrow()
    {
        // Kích hoạt Trigger để Animator chuyển từ "AimLoop" sang "FireRelease"
        if (anim) anim.SetTrigger("FireE");

        // Sinh mũi tên
        // Mẹo: Nên delay hàm này khoảng 0.1s để khớp với động tác buông tay
        // Nhưng tạm thời để thế này cho nhạy
        SpawnArrow();

        // Tính hồi chiêu
        StartCooldown("Skill_E", cooldownE);

        // Reset trạng thái ngắm ngay lập tức để không bị kẹt
        isAiming = false;
        if (anim) anim.SetBool("IsAiming", false);
    }

    void SpawnArrow()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            // Bắn ra mũi tên
            Instantiate(arrowPrefab, firePoint.position, transform.rotation);
        }
    }
    
    // ... (Giữ nguyên phần Skill Q và hàm khác) ...
    void UseSkillQ()
    {
         if (anim) anim.SetTrigger("ArcherSkillQ");
         currentRage -= rageCostQ;
         StartCooldown("Skill_Q", cooldownQ);
    }
}