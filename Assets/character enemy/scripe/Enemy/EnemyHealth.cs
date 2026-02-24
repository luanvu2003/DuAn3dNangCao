using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider;
    private Animator animator;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Tự lấy Animator ở object con nếu có
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
            // 🔥 Tắt thanh máu ngay khi hết máu
            if (healthSlider != null)
                healthSlider.gameObject.SetActive(false);

            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (animator != null)
        {
            animator.applyRootMotion = false;

            // Ép vào state Die ngay lập tức
            animator.Play("Die", 0, 0f);

            // 🔥 Sau 0.1s khóa animator lại
            StartCoroutine(FreezeAnimator());
        }

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator FreezeAnimator()
    {
        // Đợi animation Die chạy xong
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length
        );

        animator.enabled = false;
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}