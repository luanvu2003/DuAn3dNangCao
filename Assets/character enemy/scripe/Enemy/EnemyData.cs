using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("References")]
    public Transform waypointHolder;

    [Header("Ranges")]
    public float chaseRange = 8f;     
    public float attackRange = 1.5f;

    // Hàm này sẽ trả về Transform của Player đứng gần Enemy nhất
    public Transform GetActivePlayer()
    {
        // 1. Tìm TẤT CẢ các object đang Active có tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity; // Khởi tạo khoảng cách là vô cực
        Vector3 currentPos = transform.position;

        // 2. Duyệt qua danh sách các Player tìm được
        foreach (GameObject p in players)
        {
            // (Tùy chọn: Nếu muốn kỹ hơn, có thể check xem Player này còn máu không)
            // CharacterBaseStats stats = p.GetComponent<CharacterBaseStats>();
            // if (stats != null && stats.currentHP <= 0) continue;

            // 3. Tính khoảng cách từ Enemy tới Player này
            // (Dùng sqrMagnitude để so sánh nhanh hơn Distance, nhưng dùng Distance cho dễ hiểu)
            float distToPlayer = Vector3.Distance(p.transform.position, currentPos);

            // 4. Nếu Player này gần hơn người cũ (hoặc là người đầu tiên tìm thấy)
            if (distToPlayer < closestDistance)
            {
                closestDistance = distToPlayer;
                bestTarget = p.transform;
            }
        }
        return bestTarget;
    }
}