using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI & Animation")]
    public Slider healthSlider;
    private Animator animator;

    [Header("Loot Settings")] // [MỚI] Cài đặt rớt đồ
    public GameObject itemToDrop;        // Prefab Item (Vàng, Máu...)
    public int minDropAmount = 1;        // Số lượng rớt tối thiểu
    public int maxDropAmount = 5;        // Số lượng rớt tối đa
    
    [Range(0, 100)] 
    public float dropChance = 100f;      // Tỉ lệ rớt (100 là chắc chắn rớt)
    
    public float scatterForce = 3f;      // Lực bắn tung tóe item ra xa

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (healthSlider != null)
                healthSlider.gameObject.SetActive(false);

            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // 🔥 [QUAN TRỌNG] Gọi hàm rớt đồ
        DropLoot(); 

        DisableEnemy();

        if (animator != null)
        {
            animator.applyRootMotion = false;
            animator.Play("Die", 0, 0f);
            StartCoroutine(FreezeAnimator());
        }

        StartCoroutine(DestroyAfterDelay());
    }

    // --------------------------------------------------
    // 🔥 [MỚI] HÀM RỚT NHIỀU ITEM NGẪU NHIÊN
    // --------------------------------------------------
    void DropLoot()
    {
        if (itemToDrop == null) return;

        // 1. Tính toán xem có rớt hay không dựa trên tỉ lệ
        if (Random.Range(0f, 100f) > dropChance) return;

        // 2. Random số lượng item sẽ rớt (ví dụ từ 2 đến 5 cái)
        int dropCount = Random.Range(minDropAmount, maxDropAmount + 1);

        for (int i = 0; i < dropCount; i++)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        // Tạo vị trí ngẫu nhiên xung quanh Enemy một chút để không bị chồng lên nhau
        Vector3 randomOffset = Random.insideUnitSphere * 1.0f; 
        randomOffset.y = 0.5f; // Đảm bảo nó nảy lên trên một chút

        Vector3 spawnPos = transform.position + Vector3.up + randomOffset;

        GameObject loot = Instantiate(itemToDrop, spawnPos, Quaternion.identity);

        // 🔥 Hiệu ứng nảy tung tóe (Nếu item có Rigidbody)
        Rigidbody rb = loot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Bắn item lên trời và tản ra xung quanh
            Vector3 forceDir = (Vector3.up * 1.5f) + Random.insideUnitSphere;
            rb.AddForce(forceDir * scatterForce, ForceMode.Impulse);
        }
    }

    void DisableEnemy()
    {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) { agent.isStopped = true; agent.enabled = false; }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false; // Tắt va chạm để player không vấp phải xác
    }

    IEnumerator FreezeAnimator()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false;
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Đợi 3s rồi xóa xác
        Destroy(gameObject);
    }
}