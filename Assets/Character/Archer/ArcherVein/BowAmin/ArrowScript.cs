using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public ParticleSystem hitEffect; // Hiệu ứng hạt khi va chạm

    void OnCollisionEnter(Collision collision)
    {
        // Phát hiệu ứng hạt tại vị trí va chạm
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 2f); // Huỷ hiệu ứng sau 2 giây
        }

        // Huỷ mũi tên sau khi va chạm
        Destroy(gameObject);
    }
}
