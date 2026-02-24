using UnityEngine;

public class QuaCauController : MonoBehaviour
{
    public GameObject quacauObject;           
    public ParticleSystem disappearEffect; 
    public GameObject arrowPrefab;         
    public Transform shootPoint;           
    public float arrowForce = 20f;         
    public float arrowDelay = 0.5f;

    private bool isPlaying = false;
    private float animationLength = 0f;

    void Start()
    {

    }

    void Update()
    {
        // Logic bắn thường (Chuột trái) - Giữ nguyên, chỉ thêm điều kiện check E
        // Nếu đang giữ E thì BowController này không tự xử lý chuột trái nữa (để Skill E xử lý)
        if (Input.GetKey(KeyCode.E)) return; 

        if (Input.GetMouseButtonDown(0) && !isPlaying)
        {
            ShowQuaCauAndPlayAnim(); // Gọi hàm public
            
            // Bắn thường
            Invoke(nameof(ShootArrowBasic), arrowDelay);
            Invoke(nameof(HideQuaCau), animationLength);
        }
    }

    // --- CÁC HÀM PUBLIC ĐỂ SKILL E GỌI ---

    public void ShowQuaCauAndPlayAnim()
    {
        quacauObject.SetActive(true);
        isPlaying = true;
    }

    public void HideQuaCau()
    {
        if (!quacauObject.activeSelf) return; // Nếu tắt rồi thì thôi

        if (disappearEffect != null)
        {
            disappearEffect.transform.position = quacauObject.transform.position;
            disappearEffect.Play();
        }

        quacauObject.SetActive(false);
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