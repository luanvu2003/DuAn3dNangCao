using UnityEngine;

public class MagicSkills : MonoBehaviour
{
    // Tạo Singleton để viên đạn dễ dàng tìm thấy nhân vật
    public static MagicSkills instance;

    [Header("Rage Settings (Hệ thống Nộ)")]
    public float maxRage = 100f;
    public float currentRage = 0f;
    public float ragePerHit = 10f; // Mỗi lần trúng quái tăng 10

    [Header("Skill E Settings")]
    public GameObject skillEPrefab; // Kéo Prefab Skill E vào đây
    public Transform castPoint;     // Vị trí bắn (dưới chân)
    public float cooldownE = 3f;
    private float nextSkillETime = 0f;

    [Header("Skill Q Settings (Ultimate)")]
    public float cooldownQ = 5f; // Hồi chiêu của Q (nếu cần)
    private float nextSkillQTime = 0f;

    [Header("Components")]
    public Animator anim;

    void Awake()
    {
        instance = this; // Đăng ký bản thân để script Hitbox gọi được
    }

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        
        // Nếu chưa có CastPoint, dùng tạm vị trí nhân vật
        if (castPoint == null) castPoint = transform;
    }

    void Update()
    {
        // --- XỬ LÝ SKILL E ---
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextSkillETime)
        {
            UseSkillE();
        }

        // --- XỬ LÝ SKILL Q ---
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextSkillQTime)
        {
            if (currentRage >= maxRage)
            {
                UseSkillQ();
            }
            else
            {
                Debug.Log("Chưa đủ nộ: " + currentRage + "/" + maxRage);
            }
        }
    }

    void UseSkillE()
    {
        // 1. Chạy Animation (nếu chưa có thì dòng này ko lỗi, chỉ warning nhẹ)
        if (anim != null) anim.SetTrigger("SkillE");

        // 2. SINH SKILL NGAY LẬP TỨC (Vì chưa có Anim Event)
        // Sau này có Anim, bạn cắt đoạn này ra hàm riêng để Anim Event gọi
        SpawnSkillE();

        // 3. Tính hồi chiêu
        nextSkillETime = Time.time + cooldownE;
    }

    void UseSkillQ()
    {
        // 1. Chạy Animation Q
        if (anim != null) anim.SetTrigger("SkillQ");

        // 2. Reset Nộ về 0
        currentRage = 0;
        
        // 3. Tính hồi chiêu (nếu muốn Q cũng phải đợi)
        nextSkillQTime = Time.time + cooldownQ;

        Debug.Log("ULTIMATE KÍCH HOẠT!");
    }

    // Hàm này sinh ra Prefab (đạn/vòng xoáy)
    public void SpawnSkillE()
    {
        if (skillEPrefab != null)
        {
            // Sinh ra skill tại vị trí castPoint, giữ nguyên góc xoay của Prefab gốc
            Instantiate(skillEPrefab, castPoint.position, skillEPrefab.transform.rotation);
        }
    }

    // Hàm này để script Hitbox gọi khi trúng quái
    public void AddRage()
    {
        if (currentRage < maxRage)
        {
            currentRage += ragePerHit;
            // Kẹp nộ không cho quá 100
            currentRage = Mathf.Clamp(currentRage, 0, maxRage);
            Debug.Log("Đã tăng nộ! Hiện tại: " + currentRage);
        }
    }
}