using UnityEngine;

public class MagicSkillHitbox : MonoBehaviour
{
    public string enemyTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        // 1. In ra tên của BẤT CỨ CÁI GÌ va phải (kể cả đất, tường)
        Debug.Log("Hitbox đã chạm vào: " + other.name + " | Tag: " + other.tag);

        // 2. Kiểm tra Tag
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("--> Đã xác nhận là Kẻ Địch!");
            
            if (MagicSkills.instance != null)
            {
                MagicSkills.instance.AddRage();
            }
            else
            {
                Debug.LogError("LỖI: Không tìm thấy MagicSkills.instance! (Kiểm tra xem nhân vật có script chưa)");
            }
        }
    }
}