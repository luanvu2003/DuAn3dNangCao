using UnityEngine;

public class CharacterSoundEvents : MonoBehaviour
{
    // Hàm này sẽ được Animation gọi
    public void CT_BSAT()
    {
        if (AudioManager.Instance != null)
        {
            // "Attack1" là tên bạn đặt trong list của AudioManager
            AudioManager.Instance.PlaySFX("CT_BSAT");
        }
    }

    public void CT_HIT()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("CT_HIT");
        }
    }

    // Bạn có thể thêm bước chân luôn
    public void DS_BSAT()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("DS_BSAT");
        }
    }
    public void PS_BSAT()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("PS_BSAT");
        }
    }
    public void PS_EX()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("PS_EX");
        }
    }
}