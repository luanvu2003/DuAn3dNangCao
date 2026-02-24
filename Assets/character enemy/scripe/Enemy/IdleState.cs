using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    private EnemyData data;
    private float time;

    public float idleDuration = 5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        data = animator.GetComponentInParent<EnemyData>();

        if (data == null)
        {
            Debug.LogError("EnemyData chưa được gắn lên Enemy!");
            return;
        }

        time = 0f;

        animator.SetBool("isPatrolling", false);
        animator.SetBool("isRunning", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        if (time >= idleDuration)
        {
            animator.SetBool("isPatrolling", true);
            return;
        }

        if (data.player != null)
        {
            float distance = Vector3.Distance(
                animator.transform.position,
                data.player.position
            );

            if (distance <= data.chaseRange)
            {
                animator.SetBool("isRunning", true);
            }
        }
    }
}
