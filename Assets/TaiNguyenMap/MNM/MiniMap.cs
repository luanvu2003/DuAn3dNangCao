using UnityEngine;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public List<Transform> players;   // Danh sách các player
    private float y_camera;
    private int currentPlayerIndex = 0; // Player hiện tại mà camera theo dõi

    void Start()
    {
        y_camera = transform.position.y;
    }

    void Update()
    {
        if (players.Count == 0) return;

        Transform currentPlayer = players[currentPlayerIndex];
        transform.position = new Vector3(currentPlayer.position.x, y_camera, currentPlayer.position.z);
        transform.rotation = Quaternion.Euler(90, currentPlayer.eulerAngles.y, 0);
    }

    // Hàm chuyển đổi player mà minimap camera theo dõi
    public void SwitchPlayer(int index)
    {
        if (index >= 0 && index < players.Count)
        {
            currentPlayerIndex = index;
        }
    }

    // Ví dụ: gọi hàm này khi nhấn phím Tab để đổi player
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }
    }
}
