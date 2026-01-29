using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : StateMachineBehaviour
{
    private List<Transform> waypoints = new List<Transform>();
    private NavMeshAgent agent;
    private Transform player;

    private float time;
    private int lastIndex = -1;
    private float chaseRange = 8f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0f;

        // 🔥 LUÔN CLEAR – bắt buộc với StateMachineBehaviour
        waypoints.Clear();

        // Player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        // NavMeshAgent
        agent = animator.GetComponentInParent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("❌ Không tìm thấy NavMeshAgent!");
            return;
        }

        // WayPoint holder
        GameObject holder = GameObject.FindGameObjectWithTag("WayPoint");
        if (holder == null)
        {
            Debug.LogError("❌ Không tìm thấy GameObject có tag 'WayPoint'");
            return;
        }

        // Load waypoint con
        foreach (Transform t in holder.transform)
        {
            waypoints.Add(t);
        }

        Debug.Log("✅ Waypoint loaded: " + waypoints.Count);

        if (waypoints.Count == 0)
        {
            Debug.LogError("❌ WayPoint KHÔNG có waypoint con!");
            return;
        }

        // Set điểm đầu tiên
        SetRandomDestination();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || waypoints.Count == 0) return;

        time += Time.deltaTime;
        if (time > 10f)
            animator.SetBool("isPatrolling", false);

        // Tới waypoint → chọn cái mới
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }

        // Chase player
        if (player != null)
        {
            float distance = Vector3.Distance(
                animator.transform.position,
                player.position
            );

            if (distance <= chaseRange)
            {
                animator.SetBool("isRunning", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();
    }

    private void SetRandomDestination()
    {
        if (waypoints.Count == 0 || agent == null || !agent.isOnNavMesh)
            return;

        int index;
        do
        {
            index = Random.Range(0, waypoints.Count);
        }
        while (index == lastIndex && waypoints.Count > 1);

        lastIndex = index;
        agent.SetDestination(waypoints[index].position);
    }
}
