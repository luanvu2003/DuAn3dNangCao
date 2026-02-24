using UnityEngine;
using System.Collections;

public class Pillar : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f; // Tăng máu lên 100
    private float currentHealth;

    [Header("Shake Settings")]
    public float duration = 0.2f;   
    public float magnitude = 0.2f; 

    private Vector3 originalPos;
    private bool isShaking;

    void Start()
    {
        currentHealth = maxHealth;
        originalPos = transform.localPosition;
    }

    // Đổi thành float để khớp với hệ thống damage của Player
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Cột bị đánh! Máu còn: " + currentHealth);

        // Rung mỗi lần bị đánh
        if (!isShaking)
            StartCoroutine(Shake());

        if (currentHealth <= 0)
        {
            DestroyPillar();
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, 0, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }

    void DestroyPillar()
    {
        // Kiểm tra null để tránh lỗi nếu chưa có QuestManager
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.AddProgress(1);
        }
        
        Destroy(gameObject);
    }
}