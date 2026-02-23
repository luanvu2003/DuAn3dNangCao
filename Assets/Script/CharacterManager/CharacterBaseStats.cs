using UnityEngine;
using System.Collections.Generic;

public class CharacterBaseStats : MonoBehaviour
{
    [Header("CHỈ SỐ CƠ BẢN (BASE)")]
    public float maxHP = 1000f; // Máu tối đa
    public float currentHP;
    
    public float maxRage = 100f;
    public float currentRage;

    // Dictionary lưu cooldown: Key là tên chiêu (ví dụ "SkillE"), Value là thời gian hồi xong
    protected Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();

    protected virtual void Start()
    {
        currentHP = maxHP;
        // currentRage = 0; // Không reset nộ ở đây để giữ lại khi qua màn
    }

    // --- HỆ THỐNG COOLDOWN ---
    public void StartCooldown(string skillName, float duration)
    {
        if (skillCooldowns.ContainsKey(skillName))
            skillCooldowns[skillName] = Time.time + duration;
        else
            skillCooldowns.Add(skillName, Time.time + duration);
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
        Debug.Log(transform.name + " bị đánh! Máu còn: " + currentHP);
        if (currentHP <= 0) Die();
    }

    public virtual void AddRage(float amount)
    {
        currentRage += amount;
        if (currentRage > maxRage) currentRage = maxRage;
        Debug.Log("Nộ hiện tại: " + currentRage);
    }

    protected virtual void Die()
    {
        Debug.Log(transform.name + " Đã hy sinh!");
        // Logic chết (ragdoll, game over...)
    }
}