using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{

    [Header("World References")]
    public GameObject pillarsParent;   // Object chứa toàn bộ trụ
    public WorldSpawner worldSpawner;  // Spawner ngoài map
    public static QuestManager Instance;

    [Header("UI References")]
    public GameObject questPanel;       
    public TMP_Text questTitleText;
    public TMP_Text questProgressText;

    [Header("Story UI")]
    public GameObject acceptButton;     // Nút "Chấp nhận"

    private int currentAmount = 0;
    private int targetAmount = 5;
    private bool questActive = false;

    void Awake()
    {
        Instance = this;
        questPanel.SetActive(false);
        if (acceptButton != null) acceptButton.SetActive(false);
    }

    // --- GỌI HÀM NÀY KHI GẶP NPC ---
    public void ShowQuestStory()
    {
        questPanel.SetActive(true);
        questTitleText.text = "Trưởng làng: Hãy phá hủy 5 Trụ Bóng Tối!";
        questProgressText.text = "Bạn có đồng ý giúp đỡ không?";

        if (acceptButton != null) acceptButton.SetActive(true);

        // 🔥 BẬT CỜ -> HIỆN CHUỘT
        if (GameCursorManager.Instance != null)
            GameCursorManager.Instance.isStoryUIOpen = true;
    }

    // --- GÁN HÀM NÀY VÀO NÚT "CHẤP NHẬN" ---
public void OnClickAcceptQuest()
{
    if (acceptButton != null) acceptButton.SetActive(false);
        
    StartDestroyPillarQuest();

    // 🔥 Bật trụ
    if (pillarsParent != null)
        pillarsParent.SetActive(true);

    // 🔥 Bật spawn quái
    if (worldSpawner != null)
        worldSpawner.StartSpawning();

    if (GameCursorManager.Instance != null)
        GameCursorManager.Instance.isStoryUIOpen = false;
}

    public void StartDestroyPillarQuest()
    {
        questActive = true;
        currentAmount = 0;
        targetAmount = 5;
        questPanel.SetActive(true);
        questTitleText.text = "Nhiệm vụ: Phá hủy Trụ Bóng Tối";
        UpdateUI();
    }

    public void AddProgress(int amount)
    {
        if (!questActive) return;
        currentAmount += amount;
        UpdateUI();
        if (currentAmount >= targetAmount) CompleteQuest();
    }

    void UpdateUI() { questProgressText.text = currentAmount + " / " + targetAmount; }

    void CompleteQuest()
    {
        questProgressText.text = "Hoàn thành!";
        questActive = false;
        Invoke("HidePanel", 3f);
    }

    void HidePanel()
    {
        questPanel.SetActive(false);
        // Đảm bảo tắt cờ
        if (GameCursorManager.Instance != null)
            GameCursorManager.Instance.isStoryUIOpen = false;
    }
}