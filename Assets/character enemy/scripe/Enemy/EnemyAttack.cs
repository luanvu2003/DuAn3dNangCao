using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum AttackType
    {
        Ranged,
        Melee
    }

    public AttackType attackType;

    [Header("Ranged Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public int rangedDamage = 10;

    [Header("Melee Settings")]
    public Collider meleeHitbox; // Kéo Collider ở tay quái vào đây
    public int meleeDamage = 20;

    private EnemyMeleeDamage meleeScript;

    void Start()
    {
        // Nếu là cận chiến, tự tìm script damage trên hitbox để set số dame
        if (meleeHitbox != null)
        {
            meleeScript = meleeHitbox.GetComponent<EnemyMeleeDamage>();
            if (meleeScript != null)
            {
                meleeScript.damage = meleeDamage;
            }
        }
    }

    // ================= RANGED (ĐƯỢC GỌI BỞI ANIMATION EVENT) =================
    public void Shoot()
    {
        if (attackType != AttackType.Ranged) return;
        if (fireballPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        Fireballl fb = bullet.GetComponent<Fireballl>();
        if (fb != null)
        {
            fb.SetDamage(rangedDamage); // Truyền dame vào đạn
        }
    }

    // ================= MELEE (ĐƯỢC GỌI BỞI ANIMATION EVENT) =================
    public void EnableHitbox()
    {
        if (attackType != AttackType.Melee) return;
        if (meleeHitbox != null) meleeHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        if (attackType != AttackType.Melee) return;
        if (meleeHitbox != null) meleeHitbox.enabled = false;
    }
}