using UnityEngine;
using UnityEngine.AI;

public class RunState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    public float attackRange = 1.3f;
    public float stopChaseDistance = 15f;
    public float runSpeed = 4f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        agent = animator.GetComponentInParent<NavMeshAgent>();
        if (agent == null) return;

        agent.isStopped = false;
        agent.speed = runSpeed;              // 🔥 chạy nhanh hơn walk
        agent.stoppingDistance = attackRange - 0.1f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || player == null) return;

        float distance = Vector3.Distance(
            animator.transform.position,
            player.position
        );

        // 🔥 DÍ THEO PLAYER MỖI FRAME
        agent.SetDestination(player.position);

        // Vào attack
        if (distance <= attackRange)
        {
            agent.isStopped = true;           // 🔥 BẮT BUỘC
            animator.SetBool("isAttacking", true);
            animator.SetBool("isRunning", false);
            return;
        }

        // Mất dấu → quay lại patrol
        if (distance > stopChaseDistance)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isPatrolling", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
        {
            agent.isStopped = false;   // 🔥 mở lại cho state sau
        }
    }
}
