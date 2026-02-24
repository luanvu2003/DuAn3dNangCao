using UnityEngine;

public class Fireballl : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDir;

    private int damage;

    public void SetDamage(int dmg)
    {
        damage = dmg;
        Debug.Log("Damage được set = " + damage);
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
    Debug.Log("Fireball gây damage = " + damage);

    if (other.CompareTag("Player"))
    {
        Debug.Log("Đúng là Player!");

        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("Có PlayerHealth!");
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
}