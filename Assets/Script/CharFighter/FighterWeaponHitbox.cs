using UnityEngine;

public class FighterWeaponHitbox : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public float damage = 20f;
    public float rageGain = 15f; // Đấm trúng hồi nộ

    // Chỉ gây dame khi nhân vật đang thực hiện hành động đánh
    // (Cần xử lý bật/tắt collider qua Animation Event thì chuẩn nhất, 
    // nhưng ở đây mình làm đơn giản là luôn check)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Fighter đấm trúng: " + other.name);

            // 1. Gây dame (Gọi script máu của Enemy)
            // other.GetComponent<EnemyHealth>()?.TakeDamage(damage);

            // 2. Cộng nộ (Gọi về Instance của FighterSkills)
            if (FighterSkills.instance != null)
            {
                FighterSkills.instance.AddRage(rageGain);
            }
            
            // Effect máu me tung tóe có thể spawn ở đây
        }
    }
}