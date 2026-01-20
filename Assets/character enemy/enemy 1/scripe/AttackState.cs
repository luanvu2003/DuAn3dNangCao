using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Transform player;
    bool hasRotated;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hasRotated = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1️⃣ Chỉ xoay 1 lần khi mới bắt đầu attack
        if (!hasRotated)
        {
            Vector3 direction = player.position - animator.transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                animator.transform.rotation = Quaternion.LookRotation(direction);
            }

            hasRotated = true;
        }

        // 2️⃣ Khi animation đánh gần xong
        if (stateInfo.normalizedTime >= 0.95f)
        {
            float distance = Vector3.Distance(player.position, animator.transform.position);

            if (distance <= 2.5f)
            {
                // Đánh tiếp → reset animation
                animator.Play(stateInfo.fullPathHash, 0, 0f);
                hasRotated = false; // cho phép xoay lại lần sau
            }
            else
            {
                // Player ra xa → ngừng attack
                animator.SetBool("isAttacking", false);
            }
        }
    }
}
