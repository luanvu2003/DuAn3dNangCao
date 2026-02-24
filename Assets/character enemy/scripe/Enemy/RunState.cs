using UnityEngine;
using UnityEngine.AI;

public class RunState : StateMachineBehaviour
{
    NavMeshAgent agent;
    EnemyData data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponentInParent<NavMeshAgent>();
        data  = animator.GetComponentInParent<EnemyData>();

        if (agent == null || data == null) return;

        agent.isStopped = false;
        agent.speed = 4f;
        agent.stoppingDistance = data.attackRange; // 🔥 dừng đúng tầm đánh
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || data == null) return;

        // --- SỬA LỖI: Tìm mục tiêu đang sống ---
        Transform target = data.GetActivePlayer();

        // Nếu không tìm thấy ai (chết hết hoặc tắt hết) -> Về đi tuần
        if (target == null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isPatrolling", true);
            return;
        }

        // --- Thay data.player bằng biến target ---
        float distance = Vector3.Distance(
            animator.transform.position,
            target.position
        );

        // 👉 CHƯA VÀO TẦM → ĐUỔI
        if (distance > data.attackRange)
        {
            agent.SetDestination(target.position);
        }

        // ✅ VÀO TẦM → ĐÁNH
        if (distance <= data.attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
            return;
        }

        // ❌ Mất dấu (Xa quá) -> Về đi tuần
        if (distance > data.chaseRange)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isPatrolling", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
    }
}