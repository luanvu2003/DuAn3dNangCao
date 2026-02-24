using UnityEngine;

public class Fireballl : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDir;

    private int damage = 10; // Mặc định 10 nếu quên set

    public void SetDamage(int dmg)
    {
        damage = dmg;
        // Debug.Log("Damage được set = " + damage);
    }

    void Start()
    {
        moveDir = transform.forward;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Chỉ xử lý nếu trúng Player
        if (other.CompareTag("Player"))
        {
            // --- SỬA Ở ĐÂY: Tìm CharacterBaseStats thay vì PlayerHealth ---
            CharacterBaseStats playerStats = other.GetComponent<CharacterBaseStats>();

            // Nếu không tìm thấy ở vật va chạm, thử tìm ở cha nó (Character Controller thường bọc Collider)
            if (playerStats == null)
            {
                playerStats = other.GetComponentInParent<CharacterBaseStats>();
            }

            if (playerStats != null)
            {
                Debug.Log("Fireball trúng Player! Gây dame: " + damage);
                playerStats.TakeDamage(damage); // Trừ máu và cập nhật UI
            }

            Destroy(gameObject); // Hủy đạn
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Untagged"))
        {
             // Hủy đạn nếu trúng tường/đất (trừ Enemy ra để không tự bắn mình)
             Destroy(gameObject);
        }
    }
}