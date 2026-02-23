using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDir;

    private int damage;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    void Start()
    {
        moveDir = transform.forward;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}