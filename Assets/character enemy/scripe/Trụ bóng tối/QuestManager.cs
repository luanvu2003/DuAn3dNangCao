using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public GameObject questPanel;
    public TMP_Text questTitleText;
    public TMP_Text questProgressText;

    private int currentAmount = 0;
    private int targetAmount = 5;

    private bool questActive = false;

    void Awake()
    {
        Instance = this;
        questPanel.SetActive(false);
    }

    public void StartDestroyPillarQuest()
    {
        questActive = true;
        currentAmount = 0;
        targetAmount = 5;

        questPanel.SetActive(true);

        questTitleText.text = "Phá hủy 5 Trụ Bóng Tối";
        UpdateUI();
    }

    public void AddProgress(int amount)
    {
        if (!questActive) return;

        currentAmount += amount;

        if (currentAmount > targetAmount)
            currentAmount = targetAmount;

        UpdateUI();

        if (currentAmount >= targetAmount)
        {
            CompleteQuest();
        }
    }

    void UpdateUI()
    {
        questProgressText.text = currentAmount + " / " + targetAmount;
    }

    void CompleteQuest()
    {
        questProgressText.text = "Hoàn thành!";
        questActive = false;

        // 🔥 nếu muốn tự tắt sau 3s
        Invoke("HidePanel", 3f);
    }

    void HidePanel()
    {
        questPanel.SetActive(false);
    }
}