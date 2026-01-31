using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    EnemyData data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponentInParent<NavMeshAgent>();
        data  = animator.GetComponentInParent<EnemyData>();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.updateRotation = false;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (data == null || data.player == null) return;

        // 🔄 quay mặt về player (chỉ trục Y)
        Vector3 dir = data.player.position - animator.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            animator.transform.rotation =
                Quaternion.Slerp(animator.transform.rotation, rot, Time.deltaTime * 8f);
        }

        float distance = Vector3.Distance(
            animator.transform.position,
            data.player.position
        );

        // ❌ player ra khỏi tầm → quay lại run
        if (distance > data.attackRange)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }
    }
}
