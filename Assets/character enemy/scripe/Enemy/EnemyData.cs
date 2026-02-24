using UnityEngine;
// Không cần using System.Collections.Generic nữa nếu dùng mảng

public class EnemyData : MonoBehaviour
{
    [Header("References")]
    public Transform waypointHolder;

    // ĐỔI LIST THÀNH ARRAY []
    public Transform[] players; 

    [Header("Ranges")]
    public float chaseRange = 8f;     
    public float attackRange = 1.5f;

    public Transform GetActivePlayer()
    {
        // Duyệt mảng cũng y hệt duyệt List
        foreach (Transform p in players)
        {
            if (p != null && p.gameObject.activeInHierarchy)
            {
                return p;
            }
        }
        return null; 
    }
}