using UnityEngine;
// Thêm namespace này để gọi được script kia
using Benjathemaker; 

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    public int scoreValue = 10;
    public GameObject pickupEffect;

    [Header("Magnet Settings")]
    public float detectRange = 5f;       
    public float moveSpeed = 15f;        
    public float collectDistance = 0.8f; 

    [Header("Inventory Data")]
    public ItemData itemData;

    private Transform playerTransform;
    private bool isBeingPulled = false;
    private Rigidbody rb;
    private Collider col;
    
    // Biến để chứa script animation
    private SimpleGemsAnim animScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        // 1. Tìm script SimpleGemsAnim đang gắn trên vật phẩm
        animScript = GetComponent<SimpleGemsAnim>();

        // Cài đặt vật lý ban đầu để rớt xuống đất đẹp
        rb.useGravity = true; 
        rb.isKinematic = false; 
    }

    void Update()
    {
        FindPlayer();

        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // 2. Nếu lọt vào tầm hút
            if (distance <= detectRange)
            {
                isBeingPulled = true;
            }

            if (isBeingPulled)
            {
                // 🔥 QUAN TRỌNG NHẤT: Tắt script Animation đi
                // Để nó không ép vật phẩm đứng yên một chỗ nữa
                if (animScript != null && animScript.enabled)
                {
                    animScript.enabled = false;
                }

                // 🔥 Tắt vật lý để bay cho mượt
                rb.useGravity = false;
                rb.isKinematic = true; 
                if(col != null) col.isTrigger = true; 

                // Bay về phía player
                // Cộng thêm Vector3.up * 1.0f để bay vào ngực/bụng thay vì bay vào chân
                Vector3 targetPos = playerTransform.position + Vector3.up * 1.0f;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                // Nhặt khi đủ gần
                if (distance <= collectDistance)
                {
                    CollectItem(playerTransform.gameObject);
                }
            }
        }
    }

    void FindPlayer()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) player = GameObject.FindGameObjectWithTag("Player1");
            if (player != null) playerTransform = player.transform;
        }
    }

    void CollectItem(GameObject player)
    {
        if (itemData != null && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemData);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}