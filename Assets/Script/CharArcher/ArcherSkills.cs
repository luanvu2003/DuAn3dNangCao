using UnityEngine;

public class ArcherSkills : CharacterBaseStats 
{
    public static ArcherSkills instance;

    [Header("Skill E Settings")]
    public GameObject arrowPrefab; 
    public Transform firePoint;     
    public float cooldownE = 3f;
    public float maxChargeTime = 2f; // Thời gian gồng tối đa để đạt kích thước max
    public float maxScale = 3f;      // Kích thước tối đa (gấp 3 lần)

    [Header("References")]
    public BowController bowController; // Kéo script BowController vào đây

    [Header("Skill Q Settings")]
    public float cooldownQ = 5f; 
    public float rageCostQ = 100f; 

    [Header("Components")]
    public Animator anim;

    private bool isAiming = false; 
    private float currentChargeTime = 0f; // Đếm thời gian gồng

    void OnEnable()
    {
        instance = this;
        isAiming = false;
        currentChargeTime = 0f;
    }

    protected override void Start() 
    {
        base.Start(); 
        if (anim == null) anim = GetComponent<Animator>();
        if (firePoint == null) firePoint = transform;
        
        // Tự tìm BowController nếu chưa kéo
        if (bowController == null) bowController = GetComponentInChildren<BowController>();
    }

    void Update()
    {
        // --- LOGIC SKILL E ---
        if (Input.GetKey(KeyCode.E))
        {
            if (IsSkillReady("Skill_E"))
            {
                // 1. BẮT ĐẦU NGẮM
                if (!isAiming)
                {
                    isAiming = true;
                    currentChargeTime = 0f; // Reset thời gian gồng
                    
                    if(anim) anim.SetBool("IsAiming", true);
                    
                    // HIỆN CÂY CUNG LÊN
                    if(bowController) bowController.ShowBowAndPlayAnim();
                }

                // 2. TÍNH TOÁN GỒNG (Tăng dần theo thời gian)
                currentChargeTime += Time.deltaTime;
                // Giới hạn không cho vượt quá max
                if (currentChargeTime > maxChargeTime) currentChargeTime = maxChargeTime;

                // 3. CLICK CHUỘT TRÁI ĐỂ BẮN
                if (Input.GetMouseButtonDown(0))
                {
                    FireArrow();
                }
            }
        }

        // 4. THẢ E MÀ CHƯA BẮN -> HỦY
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isAiming)
            {
                isAiming = false;
                if(anim) anim.SetBool("IsAiming", false);
                
                // Ẩn cây cung đi
                if(bowController) bowController.HideBow();
            }
        }

        // --- SKILL Q ---
        if (Input.GetKeyDown(KeyCode.Q) && IsSkillReady("Skill_Q"))
        {
            if (currentRage >= rageCostQ) UseSkillQ();
        }
    }

    void FireArrow()
    {
        if (anim) anim.SetTrigger("FireE");

        SpawnArrow(); // Sinh mũi tên to

        StartCooldown("Skill_E", cooldownE);

        isAiming = false;
        if (anim) anim.SetBool("IsAiming", false);
        
        // Bắn xong thì đợi animation xong rồi ẩn cung (dùng hàm HideBow của BowController)
        // Hoặc ẩn luôn sau 1 khoảng delay ngắn
        if(bowController) Invoke(nameof(HideBowDelayed), 0.5f);
    }

    void HideBowDelayed()
    {
        if(bowController) bowController.HideBow();
    }

    void SpawnArrow()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, transform.rotation); // Bắn thẳng theo hướng người
            
            // --- XỬ LÝ KÍCH THƯỚC (SCALE) ---
            // Tính tỉ lệ phần trăm đã gồng (0 -> 1)
            float chargeRatio = currentChargeTime / maxChargeTime; 
            // Scale từ 1 đến maxScale dựa theo tỉ lệ gồng
            float finalScale = Mathf.Lerp(1f, maxScale, chargeRatio);
            
            arrow.transform.localScale = Vector3.one * finalScale;

            // --- XỬ LÝ XUYÊN THẤU ---
            ArrowScript arrowScript = arrow.GetComponent<ArrowScript>();
            if (arrowScript != null)
            {
                arrowScript.isPiercing = true; // Bật chế độ xuyên thấu
                
                // Tùy chọn: Tăng dame theo kích thước luôn nếu thích
                // arrowScript.damage = (int)(arrowScript.damage * finalScale);
            }

            // --- LỰC BẮN ---
            // Lấy lực bắn từ BowController cho đồng bộ hoặc tự set
            float force = 20f; 
            if(bowController) force = bowController.arrowForce;

            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if(rb != null) rb.AddForce(firePoint.forward * force, ForceMode.Impulse);
            
            Destroy(arrow, 5f);
        }
    }
    
    void UseSkillQ()
    {
         if (anim) anim.SetTrigger("ArcherSkillQ");
         currentRage -= rageCostQ;
         StartCooldown("Skill_Q", cooldownQ);
    }
}