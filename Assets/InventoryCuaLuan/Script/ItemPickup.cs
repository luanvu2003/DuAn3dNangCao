using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    public int scoreValue = 10;
    public GameObject pickupEffect;

    [Header("Magnet Settings")]
    public float detectRange = 5f;       // Khoảng cách bắt đầu hút
    public float moveSpeed = 10f;       // Tốc độ bay vào người
    public float collectDistance = 0.5f; // Khoảng cách cực gần để biến mất (nhặt)

    [Header("Inventory Data")]
    public ItemData itemData;

    private Transform playerTransform;
    private bool isBeingPulled = false;

    void Update()
    {
        FindPlayer();

        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // 1. Kiểm tra nếu player ở trong vùng hút
            if (distance <= detectRange)
            {
                isBeingPulled = true;
            }

            // 2. Thực hiện việc di chuyển (hút) vật phẩm
            if (isBeingPulled)
            {
                // Bay về phía player
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

                // 3. Khi chạm sát người thì nhặt
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
        // Kiểm tra PlayerStats (đã thêm check null an toàn để không bị lỗi đỏ)
        // Nếu bạn không có script PlayerStats, dòng này sẽ bị bỏ qua thay vì làm crash game
        var playerStats = player.GetComponent<MonoBehaviour>(); // Thay đổi tạm thời hoặc tạo script PlayerStats
        
        /* LƯU Ý: Nếu bạn đã xóa PlayerStats, hãy comment đoạn này lại 
           hoặc tạo 1 file PlayerStats.cs mới.
        */
        // player.GetComponent<PlayerStats>()?.AddScore(scoreValue);

        if (itemData != null && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemData);
            Debug.Log("Đã nhặt tự động: " + itemData.itemName);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}