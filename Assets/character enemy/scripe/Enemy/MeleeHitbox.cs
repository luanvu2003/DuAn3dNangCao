using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private EnemyAttack enemyAttack;

    void Start()
    {
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(enemyAttack.meleeDamage);
                Debug.Log("Enemy melee trúng player!");
            }
        }
    }
}