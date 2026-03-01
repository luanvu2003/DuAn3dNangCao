using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("--- Audio Sources ---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("--- Background Music ---")]
    public AudioClip backgroundMusic; // Nhạc nền (Chỉ 1 bài như bạn yêu cầu)

    [Header("--- Sound Effects List ---")]
    // Dùng List tùy chỉnh để bạn đặt tên cho dễ nhớ (ví dụ: "Jump", "Attack")
    public List<SoundItem> sfxList; 

    void Awake()
    {
        // Setup Singleton + DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Tự động phát nhạc nền khi vào game
        PlayMusic(backgroundMusic);
    }

    // --- PHÁT NHẠC (MUSIC) ---
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true; // Nhạc nền phải lặp lại
        musicSource.Play();
    }

    // --- PHÁT HIỆU ỨNG (SFX) THEO TÊN ---
    // Ví dụ gọi: AudioManager.Instance.PlaySFX("Jump");
    public void PlaySFX(string name)
    {
        SoundItem s = sfxList.Find(item => item.name == name);
        if (s != null && s.clip != null)
        {
            // PlayOneShot giúp các âm thanh đè lên nhau được (ví dụ bắn súng liên thanh)
            sfxSource.PlayOneShot(s.clip);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy âm thanh tên: " + name);
        }
    }

    // --- CHỈNH ÂM LƯỢNG (Dùng cho UI Slider) ---
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

// Class phụ để hiển thị đẹp trên Inspector
[System.Serializable]
public class SoundItem
{
    public string name;      // Tên âm thanh (VD: Attack, Jump, Die)
    public AudioClip clip;   // File âm thanh
}