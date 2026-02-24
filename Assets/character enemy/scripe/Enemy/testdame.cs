using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public EnemyHealth enemy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemy.TakeDamage(15);
        }
    }
}