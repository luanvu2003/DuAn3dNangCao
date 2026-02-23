using UnityEngine;

public class ArcherArrowHitbox : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public float rageAmount = 10f; // Nộ nhận được mỗi hit

    private CharacterBaseStats myOwner; // Chủ nhân của hitbox này

    void Start()
    {
        // Tự tìm script chỉ số trên người nhân vật (tìm ở cha)
        myOwner = GetComponentInParent<CharacterBaseStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            // Cách 1: Nếu hitbox gắn trên người -> Gọi thẳng script cha
            if (myOwner != null)
            {
                myOwner.AddRage(rageAmount);
            }
            // Cách 2: Nếu là đạn bắn ra xa (không tìm được cha) -> Dùng Instance (chấp nhận rủi ro nhỏ khi swap nhanh)
            else if (MagicSkills.instance != null)
            {
                MagicSkills.instance.AddRage(rageAmount);
            }
        }
    }
}