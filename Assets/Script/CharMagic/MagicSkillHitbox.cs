using UnityEngine;

public class MagicHitbox : MonoBehaviour
{
    public int damage = 25; // Sát thương phép
    public float speed = 10f; // Tốc độ bay (nếu chưa có script bay)
    public float lifeTime = 3f; // Thời gian tồn tại

    void Start()
    {
        Destroy(gameObject, lifeTime); // Tự hủy sau 3s nếu không trúng gì
    }

    void Update()
    {
        // Bay thẳng về phía trước
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Nếu trúng Enemy
        if (other.CompareTag("Enemy"))
        {
            // Tìm máu của Enemy
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Magic hit Enemy: " + damage);
            }

            // Cộng nộ cho Magic (Gọi về instance của MagicSkills)
            if (MagicSkills.instance != null)
            {
                MagicSkills.instance.AddRage(10f);
            }

            // Hủy cục đạn sau khi trúng
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Untagged"))
        {
            // Trúng tường/đất thì hủy luôn
            Destroy(gameObject);
        }
    }
}