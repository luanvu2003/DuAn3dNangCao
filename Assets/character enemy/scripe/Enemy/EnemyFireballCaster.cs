using UnityEngine;

public class EnemyFireballCaster : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public int damage = 15; // Thêm biến damage

    public void ShootFireball()
    {
        if (fireballPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        
        // Lấy script Fireballl và set damage
        Fireballl fb = bullet.GetComponent<Fireballl>();
        if (fb != null)
        {
            fb.SetDamage(damage);
        }
    }
}