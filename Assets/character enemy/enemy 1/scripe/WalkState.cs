using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : StateMachineBehaviour
{
    private List<Transform> waypoints = new List<Transform>();
    private NavMeshAgent agent;
    private float time;
    private int lastIndex = -1;
    Transform player;
    float chaseRange = 8;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Lấy NavMeshAgent từ cha (Enemy nằm trong object khác)
        agent = animator.GetComponentInParent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Không tìm thấy NavMeshAgent trên GameObject hoặc cha của Animator!");
            return;
        }

        // Load waypoint nếu chưa có
        if (waypoints.Count == 0)
        {
            // LƯU Ý: Tag phải đúng với cái bạn gắn trong Editor
            GameObject holder = GameObject.FindGameObjectWithTag("WayPoint"); // hoặc "WayPoints" nếu bạn dùng tag đó
            if (holder == null)
            {
                Debug.LogError("Không tìm thấy object có tag 'Waypoint'! Kiểm tra Tag (không phải Name).");
                return;
            }

            foreach (Transform t in holder.transform)
            {
                if (t != holder.transform) // bỏ qua chính holder
                    waypoints.Add(t);
            }

            // if (waypoints.Count == 0)
            // {
            //     Debug.LogError("Danh sách waypoint rỗng! Hãy thêm các Transform con vào object 'Waypoint'.");
            //     return;
            // }
        }

        // Set đường đi random an toàn
        SetRandomDestination();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null) return;

        time += Time.deltaTime;
        if (time > 10f)
            animator.SetBool("isPatrolling", false);

        // Khi tới waypoint → random cái mới
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }
        float distance = Vector3.Distance(player.position,animator.transform.position);
        if(distance <= chaseRange)
        {
            animator.SetBool("isRunning", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
            agent.ResetPath();
    }

    private void SetRandomDestination()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Không có waypoint để di chuyển!");
            return;
        }

        int index;
        if (waypoints.Count == 1)
        {
            index = 0;
        }
        else
        {
            // Tránh chọn lại cùng waypoint liên tiếp
            do
            {
                index = Random.Range(0, waypoints.Count);
            } while (index == lastIndex);
        }

        lastIndex = index;
        Vector3 target = waypoints[index].position;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(target);
        }
        else
        {
            Debug.LogError("NavMeshAgent không đứng trên NavMesh! Hãy đảm bảo vị trí Enemy nằm trong vùng NavMesh đã bake.");
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

