using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Thanh Chỉ Số")]
    public Slider healthSlider; // Kéo Slider Máu vào
    public Slider rageSlider;   // Kéo Slider Nộ vào

    [Header("Skill UI")]
    public Image skillE_Icon;   // Kéo Image (Filled) của Skill E
    public Image skillQ_Icon;   // Kéo Image (Filled) của Skill Q

    void Awake()
    {
        instance = this;
    }

    // Cập nhật thanh máu
    public void UpdateHealth(float current, float max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }

    // Cập nhật thanh nộ
    public void UpdateRage(float current, float max)
    {
        if (rageSlider != null)
        {
            rageSlider.maxValue = max;
            rageSlider.value = current;
        }
    }

    // Cập nhật vòng hồi chiêu (0 là chưa hồi, 1 là đã hồi)
    public void UpdateCooldownE(float fillAmount)
    {
        if (skillE_Icon != null) skillE_Icon.fillAmount = fillAmount;
    }

    public void UpdateCooldownQ(float fillAmount)
    {
        if (skillQ_Icon != null) skillQ_Icon.fillAmount = fillAmount;
    }
}