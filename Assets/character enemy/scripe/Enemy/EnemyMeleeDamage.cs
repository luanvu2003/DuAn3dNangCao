using UnityEngine;

public class EnemyMeleeDamage : MonoBehaviour
{
    public int damage = 20; // Sát thương mặc định
    public string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Tìm script máu của Player
            CharacterBaseStats playerStats = other.GetComponent<CharacterBaseStats>();

            if (playerStats == null)
            {
                playerStats = other.GetComponentInParent<CharacterBaseStats>();
            }

            if (playerStats != null)
            {
                Debug.Log("Quái chém trúng Player! Mất: " + damage);
                playerStats.TakeDamage(damage);
                
                // Tắt Collider ngay sau khi đánh trúng để tránh gây dame nhiều lần trong 1 hit
                // (EnemyAttack sẽ bật lại nó ở lần đánh sau)
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}