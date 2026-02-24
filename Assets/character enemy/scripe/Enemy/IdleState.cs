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
            // Debug.LogError("EnemyData chưa được gắn lên Enemy!");
            return;
        }

        time = 0f;

        // Reset các trạng thái khác khi vào Idle
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (data == null) return;

        // 1. Tính thời gian đứng chờ
        time += Time.deltaTime;

        if (time >= idleDuration)
        {
            // Hết thời gian chờ -> Chuyển sang đi tuần
            animator.SetBool("isPatrolling", true);
            return;
        }

        // 2. Tìm người chơi đang active (Thay cho data.player cũ)
        Transform target = data.GetActivePlayer();

        // Nếu tìm thấy người chơi
        if (target != null)
        {
            float distance = Vector3.Distance(
                animator.transform.position,
                target.position // Dùng vị trí của target vừa tìm được
            );

            // Nếu người chơi vào tầm đuổi -> Chuyển sang chạy đuổi
            if (distance <= data.chaseRange)
            {
                animator.SetBool("isRunning", true);
            }
        }
    }
}