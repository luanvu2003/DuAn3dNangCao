using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject bowObject;           
    public Animator bowAnimator;           
    public ParticleSystem disappearEffect; 
    public GameObject arrowPrefab;         
    public Transform shootPoint;           
    public float arrowForce = 20f;         
    public float arrowDelay = 0.5f;

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
        // Logic bắn thường (Chuột trái) - Giữ nguyên, chỉ thêm điều kiện check E
        // Nếu đang giữ E thì BowController này không tự xử lý chuột trái nữa (để Skill E xử lý)
        if (Input.GetKey(KeyCode.E)) return; 

        if (Input.GetMouseButtonDown(0) && !isPlaying)
        {
            ShowBowAndPlayAnim(); // Gọi hàm public
            
            // Bắn thường
            Invoke(nameof(ShootArrowBasic), arrowDelay);
            Invoke(nameof(HideBow), animationLength);
        }
    }

    // --- CÁC HÀM PUBLIC ĐỂ SKILL E GỌI ---

    public void ShowBowAndPlayAnim()
    {
        bowObject.SetActive(true);
        bowAnimator.Play("Take 001", -1, 0f);
        isPlaying = true;
    }

    public void HideBow()
    {
        if (!bowObject.activeSelf) return; // Nếu tắt rồi thì thôi

        if (disappearEffect != null)
        {
            disappearEffect.transform.position = bowObject.transform.position;
            disappearEffect.Play();
        }

        bowObject.SetActive(false);
        isPlaying = false;
    }

    // Hàm bắn thường (Private)
    void ShootArrowBasic()
    {
        if (arrowPrefab != null && shootPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            arrow.transform.Rotate(0, -90, 0); 
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(shootPoint.forward * arrowForce, ForceMode.Impulse);
            Destroy(arrow, 3f);
        }
    }
}