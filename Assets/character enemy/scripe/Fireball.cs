using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDir;

    void Start()
    {
        moveDir = transform.forward;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }
}
