using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    public int damage = 10;  // chỉnh trong Inspector mỗi enemy khác nhau

    public void Shoot()
    {
        GameObject bullet = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        Fireball fb = bullet.GetComponent<Fireball>();
        if (fb != null)
        {
            fb.SetDamage(damage);
        }
    }
}