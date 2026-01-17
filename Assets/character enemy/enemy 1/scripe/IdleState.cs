using UnityEngine;

public class IdleState : StateMachineBehaviour
{ 
    Transform player;
    float chaseRange = 8;
    float time;

    public float idleDuration = 5f; // Thời gian đứng yên trước khi Patrol

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator.SetBool("isPatrolling", false); // đảm bảo Idle luôn là đứng yên
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        if (time >= idleDuration)
        {
            animator.SetBool("isPatrolling", true); // chuyển sang trạng thái đi tuần
        }
        float distance = Vector3.Distance(player.position,animator.transform.position);
        if(distance <= chaseRange)
        {
            animator.SetBool("isRunning", true);
        }
    }


    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
