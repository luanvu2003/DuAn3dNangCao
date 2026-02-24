using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject bowObject;           // Cây cung
    public Animator bowAnimator;           // Animator của cây cung
    public ParticleSystem disappearEffect; // Hiệu ứng hạt
    public GameObject arrowPrefab;         // Prefab mũi tên
    public Transform shootPoint;           // Vị trí đầu cung để bắn mũi tên
    public float arrowForce = 20f;         // Lực bắn mũi tên
    public float arrowDelay = 1f;
    private bool isPlaying = false;
    private float animationLength = 0f;

    void Start()
    {
        if (bowAnimator.runtimeAnimatorController.animationClips.Length > 0)
        {
            animationLength = bowAnimator.runtimeAnimatorController.animationClips[0].length;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPlaying)
        {
            bowObject.SetActive(true);
            bowAnimator.Play("Take 001", -1, 0f);
            isPlaying = true;

            // Bắn mũi tên
            Invoke(nameof(ShootArrow), arrowDelay);

            Invoke(nameof(HideBow), animationLength);
        }
    }

    void ShootArrow()
    {
        if (arrowPrefab != null && shootPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            arrow.transform.Rotate(0, 0, 0); // ví dụ xoay để mũi tên nằm theo Z
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(shootPoint.forward * arrowForce, ForceMode.Impulse);
            }
            // Tự huỷ mũi tên sau 5 giây để tránh rác
            Destroy(arrow, 3f);
        }
    }

    void HideBow()
    {
        if (disappearEffect != null)
        {
            disappearEffect.transform.position = bowObject.transform.position;
            disappearEffect.Play();
        }

        bowObject.SetActive(false);
        isPlaying = false;
    }
}
