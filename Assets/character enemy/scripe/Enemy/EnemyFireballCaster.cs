using UnityEngine;

public class EnemyFireballCaster : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    public void ShootFireball()
    {
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }
}
