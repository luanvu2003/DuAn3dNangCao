using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    EnemyData data;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lấy các component cần thiết
        agent = animator.GetComponentInParent<NavMeshAgent>();
        data  = animator.GetComponentInParent<EnemyData>();

        if (agent != null)
        {
            agent.isStopped = true;      // Dừng di chuyển khi đánh
            agent.updateRotation = false; // Tắt tự xoay của NavMesh để mình tự xoay tay
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (data == null) return;

        // --- SỬA LỖI Ở ĐÂY ---
        // Thay vì dùng data.player (đã bị xóa), ta gọi hàm tìm người đang chơi
        Transform target = data.GetActivePlayer();

        // Nếu không tìm thấy ai (chết hết hoặc lỗi), thì thôi không làm gì
        if (target == null) 
        {
            // Tùy chọn: Có thể cho về Idle nếu mất mục tiêu
            animator.SetBool("isAttacking", false);
            return;
        }

        // --- CÁC ĐOẠN DƯỚI THAY data.player BẰNG BIẾN target ---

        // 🔄 Quay mặt về phía target (chỉ trục Y)
        Vector3 dir = target.position - animator.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            animator.transform.rotation = 
                Quaternion.Slerp(animator.transform.rotation, rot, Time.deltaTime * 8f);
        }

        // Tính khoảng cách tới mục tiêu hiện tại
        float distance = Vector3.Distance(
            animator.transform.position,
            target.position
        );

        // ❌ Nếu mục tiêu chạy ra khỏi tầm đánh → Quay lại trạng thái chạy (Run)
        if (distance > data.attackRange)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;      // Cho phép đi lại
            agent.updateRotation = true;  // Trả lại quyền xoay cho NavMesh
        }
    }
}