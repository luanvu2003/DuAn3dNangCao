using UnityEngine;

public class FighterSkills : CharacterBaseStats 
{
    public static FighterSkills instance;

    [Header("Skill E Settings")]
    public GameObject skillEPrefab; 
    public Transform castPoint;     
    public float cooldownE = 3f;

    [Header("Skill Q Settings (Ultimate)")]
    public float cooldownQ = 5f; 
    public float rageCostQ = 100f; 

    [Header("Components")]
    public Animator anim;
    public Rigidbody rb; // Cần cái này để check tốc độ

    void OnEnable()
    {
        instance = this;
    }

    protected override void Start() 
    {
        base.Start(); 
        
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (castPoint == null) castPoint = transform;
    }

    void Update()
    {
        // --- XỬ LÝ SKILL E ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsSkillReady("Skill_E")) 
            {
                UseSkillE();
            }
            else
            {
                // Debug.Log("Skill E đang hồi...");
            }
        }

        // --- XỬ LÝ SKILL Q ---
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (IsSkillReady("Skill_Q"))
            {
                if (currentRage >= rageCostQ)
                {
                    UseSkillQ();
                }
                else
                {
                    Debug.Log("Chưa đủ nộ!");
                }
            }
        }
    }

    void UseSkillE()
    {
        // 1. Cập nhật trạng thái di chuyển ngay lập tức cho Animator
        // Unity 6 dùng linearVelocity, Unity cũ dùng velocity
        float speed = rb.linearVelocity.magnitude; // Hoặc rb.velocity.magnitude
        bool isMoving = speed > 0.1f;
        
        if(anim != null) 
        {
            anim.SetBool("IsMoving", isMoving); // Đồng bộ lại cho chắc
            anim.SetTrigger("FighterSkillE");   // Kích hoạt chiêu
        }
        
        // 2. Spawn skill
        SpawnSkillE();

        // 3. Hồi chiêu
        StartCooldown("Skill_E", cooldownE);
    }

    void UseSkillQ()
    {
        if (anim != null) anim.SetTrigger("FighterSkillQ");

        currentRage -= rageCostQ;
        StartCooldown("Skill_Q", cooldownQ);

        Debug.Log("ULTIMATE KÍCH HOẠT!");
    }

    public void SpawnSkillE()
    {
        if (skillEPrefab != null)
        {
            Instantiate(skillEPrefab, castPoint.position, skillEPrefab.transform.rotation);
        }
    }
}