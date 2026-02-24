using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int damage = 20;
    public ParticleSystem hitEffect;

    // Biến mới: Có xuyên thấu hay không?
    public bool isPiercing = false;

    void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
        }

        // TRƯỜNG HỢP 1: Trúng Enemy
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            SpawnEffect();

            // Logic Xuyên thấu:
            if (isPiercing)
            {
                // Nếu là xuyên thấu thì KHÔNG destroy, cứ để nó bay tiếp
                // Lưu ý: Cần tắt va chạm vật lý giữa mũi tên và quái để không bị nảy ra
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                return;
            }
        }
        Pillar pillar = collision.gameObject.GetComponent<Pillar>();
        if (pillar != null)
        {
            pillar.TakeDamage(damage);
            SpawnEffect(); // Nổ hiệu ứng nếu có
            Destroy(gameObject); // Hủy mũi tên
            return; // Kết thúc luôn để không chạy code dưới
        }
        // TRƯỜNG HỢP 2: Trúng Tường/Đất (Không phải Enemy)
        // Hoặc trúng Enemy nhưng không có xuyên thấu -> Hủy
        SpawnEffect();
        Destroy(gameObject);
    }

    void SpawnEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 2f);
        }
    }
}
