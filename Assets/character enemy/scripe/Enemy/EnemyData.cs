using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("References")]
    public Transform waypointHolder;
    public Transform player;

    [Header("Ranges")]
    public float chaseRange = 8f;
    public float attackRange = 1.5f;

    void Awake()
    {
        // Tự tìm player khi spawn
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
        }
    }
}