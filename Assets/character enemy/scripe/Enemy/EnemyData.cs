using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("References")]
    public Transform waypointHolder;

    [Header("Ranges")]
    public float chaseRange = 8f;     
    public float attackRange = 1.5f;

    public Transform GetActivePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && player.activeInHierarchy)
        {
            return player.transform;
        }

        return null;
    }
}