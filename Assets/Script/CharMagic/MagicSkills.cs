using UnityEngine;

public class MagicSkills : CharacterBaseStats 
{
    public static MagicSkills instance;

    [Header("Skill E Settings")]
    public GameObject skillEPrefab; 
    public Transform castPoint;     
    public float cooldownE = 3f;

    [Header("Skill Q Settings (Ultimate)")]
    public float cooldownQ = 5f; 
    public float rageCostQ = 100f; 

    [Header("Components")]
    public Animator anim;

    void OnEnable()
    {
        instance = this;
    }

    protected override void Start() 
    {
        base.Start(); 
        
        if (anim == null) anim = GetComponent<Animator>();
        if (castPoint == null) castPoint = transform;
    }

    // --- SỬA Ở ĐÂY ---
    // Phải đổi thành protected override void Update() để đồng bộ với cha
    // Hoặc giữ nguyên void Update() nhưng BẮT BUỘC phải có base.Update()
    protected override void Update()
    {
        base.Update(); // <--- DÒNG QUAN TRỌNG NHẤT: Gọi lớp cha để nó cập nhật UI Cooldown

        // --- XỬ LÝ SKILL E ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsSkillReady("Skill_E")) 
            {
                UseSkillE();
            }
            // else: Đang hồi
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
            }
        }
    }

    void UseSkillE()
    {
        if (anim != null) anim.SetTrigger("SkillE");
        
        SpawnSkillE();

        // Kích hoạt hồi chiêu
        StartCooldown("Skill_E", cooldownE);
    }

    void UseSkillQ()
    {
        if (anim != null) anim.SetTrigger("SkillQ");

        currentRage -= rageCostQ;
        StartCooldown("Skill_Q", cooldownQ);
        Debug.Log("MAGIC ULTIMATE!");
    }

    public void SpawnSkillE()
    {
        if (skillEPrefab != null)
        {
            // Sinh ra đạn
            Instantiate(skillEPrefab, castPoint.position, castPoint.rotation);
        }
    }
}