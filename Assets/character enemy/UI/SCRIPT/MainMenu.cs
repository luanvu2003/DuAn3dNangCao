using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionPanel;
    // Chuyển sang scene game
    public void PlayGame()
    {
        SceneManager.LoadScene("map_fx"); 
        // Đổi "GameScene" thành đúng tên scene của bạn
    }

    public void OpenOptions()
    {
        if (OptionPanel != null)
            OptionPanel.SetActive(true);
    }
    public void CloseOptions()
    {
        if (OptionPanel != null)
            OptionPanel.SetActive(false);
    }

    // Thoát game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game"); // Để test khi chạy trong Editor
    }
}