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

        if (agent == null || data == null || data.player == null) return;

        agent.isStopped = false;
        agent.speed = 4f;
        agent.stoppingDistance = data.attackRange; // 🔥 dừng đúng tầm đánh
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || data == null || data.player == null) return;

        float distance = Vector3.Distance(
            animator.transform.position,
            data.player.position
        );

        // 👉 CHƯA VÀO TẦM → ĐUỔI
        if (distance > data.attackRange)
        {
            agent.SetDestination(data.player.position);
        }

        // ✅ VÀO TẦM → ĐÁNH
        if (distance <= data.attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
            return;
        }

        // ❌ Mất dấu
        if (distance > data.chaseRange)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isPatrolling", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
            agent.isStopped = false;
    }
}
