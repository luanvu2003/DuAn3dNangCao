using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("References")]
    public Transform waypointHolder;
    public Transform player;

    [Header("Ranges")]
    public float chaseRange = 8f;     // phát hiện player
    public float attackRange = 1.5f;  // tầm đánh (mỗi enemy khác nhau)
}
