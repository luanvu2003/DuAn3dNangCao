using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum AttackType
    {
        Ranged,
        Melee
    }

    public AttackType attackType;

    [Header("Ranged")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public int rangedDamage = 10;

    [Header("Melee")]
    public Collider meleeHitbox;
    public int meleeDamage = 20;

    // ================= RANGED =================
    public void Shoot()
    {
        if (attackType != AttackType.Ranged) return;

        GameObject bullet = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        Fireballl fb = bullet.GetComponent<Fireballl>();
        if (fb != null)
        {
            fb.SetDamage(rangedDamage);
        }
    }

    // ================= MELEE =================
    public void EnableHitbox()
    {
        if (attackType != AttackType.Melee) return;

        meleeHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        if (attackType != AttackType.Melee) return;

        meleeHitbox.enabled = false;
    }
}