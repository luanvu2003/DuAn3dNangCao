using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("HP Bars")]
    public Slider healthFront;     // thanh máu chính (tụt nhanh)
    public Slider healthBack;      // thanh máu trễ (tụt chậm)

    [Header("Rage")]
    public Slider rageSlider;

    [Header("Skill UI")]
    public Image skillE_Icon;
    public Image skillQ_Icon;

    [Header("Smooth Settings")]
    public float frontSpeed = 12f;   // tốc độ thanh chính
    public float backSpeed = 2f;     // tốc độ thanh trễ

    private float targetHealth;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (healthFront != null)
        {
            healthFront.value = Mathf.Lerp(
                healthFront.value,
                targetHealth,
                Time.deltaTime * frontSpeed
            );
        }

        if (healthBack != null)
        {
            healthBack.value = Mathf.Lerp(
                healthBack.value,
                targetHealth,
                Time.deltaTime * backSpeed
            );
        }
    }

    public void UpdateHealth(float current, float max)
    {
        targetHealth = current;

        if (healthFront != null)
            healthFront.maxValue = max;

        if (healthBack != null)
            healthBack.maxValue = max;
    }

    public void UpdateRage(float current, float max)
    {
        if (rageSlider != null)
        {
            rageSlider.maxValue = max;
            rageSlider.value = current;
        }
    }

    public void UpdateCooldownE(float fillAmount)
    {
        if (skillE_Icon != null) skillE_Icon.fillAmount = fillAmount;
    }

    public void UpdateCooldownQ(float fillAmount)
    {
        if (skillQ_Icon != null) skillQ_Icon.fillAmount = fillAmount;
    }
}