using UnityEngine;

// KẾ THỪA TỪ CharacterBaseStats ĐỂ CÓ MÁU VÀ NỘ
public class MagicSkills : CharacterBaseStats 
{
    public static MagicSkills instance;

    [Header("Skill E Settings")]
    public GameObject skillEPrefab; 
    public Transform castPoint;     
    public float cooldownE = 3f;

    [Header("Skill Q Settings (Ultimate)")]
    public float cooldownQ = 5f; 
    public float rageCostQ = 100f; // Tốn bao nhiêu nộ để dùng Q?

    [Header("Components")]
    public Animator anim;

    // Khi nhân vật được BẬT lên (Swap tới), nó sẽ tự gán instance
    void OnEnable()
    {
        instance = this;
    }

    protected override void Start() // Dùng override vì lớp cha cũng có Start
    {
        base.Start(); // Gọi hàm Start của cha để set máu
        
        if (anim == null) anim = GetComponent<Animator>();
        if (castPoint == null) castPoint = transform;
    }

    void Update()
    {
        // --- XỬ LÝ SKILL E ---
        // Kiểm tra phím bấm VÀ hỏi hệ thống Cooldown xem Skill E xong chưa
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsSkillReady("Skill_E")) // "Skill_E" là tên định danh
            {
                UseSkillE();
            }
            else
            {
                Debug.Log("Skill E đang hồi! Còn: " + GetRemainingCooldown("Skill_E"));
            }
        }

        // --- XỬ LÝ SKILL Q ---
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Kiểm tra Cooldown Q VÀ Kiểm tra đủ Nộ không (currentRage có sẵn từ lớp cha)
            if (IsSkillReady("Skill_Q"))
            {
                if (currentRage >= rageCostQ)
                {
                    UseSkillQ();
                }
                else
                {
                    Debug.Log("Chưa đủ nộ: " + currentRage + "/" + rageCostQ);
                }
            }
        }
    }

    void UseSkillE()
    {
        if (anim != null) anim.SetTrigger("SkillE");
        
        // Spawn skill logic (giữ nguyên của bạn)
        SpawnSkillE();

        // KÍCH HOẠT HỒI CHIÊU CHO BASE SYSTEM
        // Dù tắt nhân vật đi, cái này vẫn đếm đúng theo thời gian thực
        StartCooldown("Skill_E", cooldownE);
    }

    void UseSkillQ()
    {
        if (anim != null) anim.SetTrigger("SkillQ");

        // Trừ nộ (Biến currentRage từ lớp cha)
        currentRage -= rageCostQ;
        
        // Tính hồi chiêu cho Q
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
    
    // Hàm AddRage đã có ở lớp cha (CharacterBaseStats), 
    // nhưng nếu bạn muốn logic riêng (ví dụ kẹp nộ), có thể Override lại.
    // Ở đây mình dùng luôn hàm của cha cho gọn.
}