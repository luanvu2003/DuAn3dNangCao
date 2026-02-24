using UnityEngine;
using System.Collections.Generic;

public class CharacterBaseStats : MonoBehaviour
{
    [Header("CHỈ SỐ CƠ BẢN")]
    public float maxHP = 100f;
    public float currentHP;
    
    public float maxRage = 100f;
    public float currentRage;

    // Dictionary lưu thời gian hồi skill: Key="TênSkill", Value="Thời điểm hồi xong"
    protected Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();
    
    // Dictionary lưu tổng thời gian cooldown (để tính % hiển thị vòng tròn)
    protected Dictionary<string, float> skillDurations = new Dictionary<string, float>();

    protected virtual void Start()
    {
        currentHP = maxHP;
        // currentRage = 0; // Tùy chọn: Giữ nộ hay reset thì tùy bạn
    }

    // 🔥 KHI SWAP TỚI NHÂN VẬT NÀY -> CẬP NHẬT UI NGAY LẬP TỨC
    protected virtual void OnEnable()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealth(currentHP, maxHP);
            UIManager.instance.UpdateRage(currentRage, maxRage);
        }
    }

    protected virtual void Update()
    {
        // 🔥 LIÊN TỤC CẬP NHẬT VÒNG HỒI CHIÊU CHO UI
        UpdateCooldownUI("Skill_E", true);  // true = Skill E
        UpdateCooldownUI("Skill_Q", false); // false = Skill Q
    }

    // Hàm tính toán hiển thị vòng tròn hồi chiêu
    void UpdateCooldownUI(string skillName, bool isSkillE)
    {
        if (UIManager.instance == null) return;

        float fill = 1f; // Mặc định là đầy (đã hồi xong)

        if (skillCooldowns.ContainsKey(skillName))
        {
            float timeDone = skillCooldowns[skillName];
            float duration = skillDurations.ContainsKey(skillName) ? skillDurations[skillName] : 1f;
            
            float remaining = timeDone - Time.time;
            
            // Nếu còn thời gian hồi -> Tính % ngược lại
            if (remaining > 0)
            {
                fill = 1f - (remaining / duration);
            }
        }

        if (isSkillE) UIManager.instance.UpdateCooldownE(fill);
        else UIManager.instance.UpdateCooldownQ(fill);
    }

    // --- HỆ THỐNG COOLDOWN ---
    public void StartCooldown(string skillName, float duration)
    {
        // Lưu thời điểm sẽ hồi xong
        if (skillCooldowns.ContainsKey(skillName))
            skillCooldowns[skillName] = Time.time + duration;
        else
            skillCooldowns.Add(skillName, Time.time + duration);

        // Lưu tổng thời gian hồi (để vẽ vòng tròn UI)
        if (skillDurations.ContainsKey(skillName))
            skillDurations[skillName] = duration;
        else
            skillDurations.Add(skillName, duration);
    }

    public bool IsSkillReady(string skillName)
    {
        if (!skillCooldowns.ContainsKey(skillName)) return true;
        return Time.time >= skillCooldowns[skillName];
    }

    public float GetRemainingCooldown(string skillName)
    {
        if (!skillCooldowns.ContainsKey(skillName)) return 0;
        float remaining = skillCooldowns[skillName] - Time.time;
        return remaining > 0 ? remaining : 0;
    }

    // --- HỆ THỐNG MÁU & NỘ ---
    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        // Cập nhật UI Máu
        if (UIManager.instance != null)
            UIManager.instance.UpdateHealth(currentHP, maxHP);

        Debug.Log(transform.name + " bị đánh! Máu còn: " + currentHP);
        
        if (currentHP <= 0) Die();
    }

    public virtual void AddRage(float amount)
    {
        currentRage += amount;
        if (currentRage > maxRage) currentRage = maxRage;

        // Cập nhật UI Nộ
        if (UIManager.instance != null)
            UIManager.instance.UpdateRage(currentRage, maxRage);
    }

    protected virtual void Die()
    {
        Debug.Log(transform.name + " Đã hy sinh!");
        // gameObject.SetActive(false); // Tạm thời tắt đi
    }
}