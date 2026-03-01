using UnityEngine;
using UnityEngine.AI;

public class WalkState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    private Vector3 startPosition;
    private float patrolRadius = 200f;
    private float time;
    private float chaseRange = 8f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lấy NavMeshAgent
        agent = animator.GetComponentInParent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("❌ Không tìm thấy NavMeshAgent!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("❌ Agent không nằm trên NavMesh!");
            return;
        }

        // Lưu vị trí spawn ban đầu
        startPosition = agent.transform.position;

        // Tìm Player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        time = 0f;

        // Chọn điểm patrol đầu tiên
        SetRandomDestination();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || !agent.isOnNavMesh) return;

        time += Time.deltaTime;

        if (time > 10f)
            animator.SetBool("isPatrolling", false);

        // Nếu đã tới điểm → chọn điểm mới
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }

        // Kiểm tra khoảng cách tới player để chuyển sang chạy
        if (player != null)
        {
            float distance = Vector3.Distance(agent.transform.position, player.position);

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
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += startPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}