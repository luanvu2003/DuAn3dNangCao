using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Chuyển sang scene game
    public void PlayGame()
    {
        SceneManager.LoadScene("map_fx"); 
        // Đổi "GameScene" thành đúng tên scene của bạn
    }

    // Thoát game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game"); // Để test khi chạy trong Editor
    }
}