using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("UI References")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // 1. Cài đặt giá trị ban đầu cho Slider dựa trên âm lượng hiện tại
        if (AudioManager.Instance != null)
        {
            // (Bạn cần chỉnh AudioSource trong AudioManager thành public hoặc viết hàm Get, 
            // nhưng ở đây tôi giả định set giá trị mặc định là 1 hoặc lấy từ PlayerPrefs nếu muốn xịn hơn)
            musicSlider.value = 1f; 
            sfxSlider.value = 1f;
        }

        // 2. Gắn sự kiện khi kéo thanh trượt
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    public void SetSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }
}