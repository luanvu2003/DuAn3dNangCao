using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueCanvas;
    public TMP_Text npcText;
    public Transform choicesParent;
    public GameObject choiceButtonPrefab;

    private DialogueNode currentNode;
    private NPCInteraction currentNPC;

    private bool isTalking = false;
    private bool canTalk = true;

    void Awake()
    {
        Instance = this;

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    public bool IsTalking()
    {
        return isTalking;
    }

    public void StartDialogue(NPCInteraction npc)
    {
        if (!canTalk || isTalking) return;

        currentNPC = npc;
        isTalking = true;
        canTalk = false;

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(true);

        // 🔥 [QUAN TRỌNG] BÁO HIỆN CHUỘT
        if (GameCursorManager.Instance != null)
        {
            GameCursorManager.Instance.isStoryUIOpen = true;
        }

        Time.timeScale = 0f; // Dừng thời gian game

        currentNode = DialogueDatabase.GetStartNode();
        ShowNode(currentNode);
    }

    void ShowNode(DialogueNode node)
    {
        npcText.text = node.text;

        // Logic nhận nhiệm vụ (Giữ nguyên của bạn)
        if (node.text.Contains("tiêu diệt đủ 5"))
        {
            if (QuestManager.Instance != null)
                QuestManager.Instance.StartDestroyPillarQuest();
            
            var spawner = FindObjectOfType<WorldSpawner>();
            if (spawner != null) spawner.StartSpawning();
        }

        // Xóa nút cũ
        foreach (Transform child in choicesParent)
            Destroy(child.gameObject);

        // Nếu hết lựa chọn -> Kết thúc thoại
        if (node.choices == null || node.choices.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }

        // Tạo nút lựa chọn mới
        foreach (DialogueChoice choice in node.choices)
        {
            GameObject btn = Instantiate(choiceButtonPrefab, choicesParent);
            btn.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

            // Xử lý click nút (Chuột đã hiện nên sẽ click được)
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                ShowNode(choice.nextNode);
            });
        }
    }

    IEnumerator EndDialogue()
    {
        // Đợi 1s (dùng Realtime vì TimeScale đang bằng 0)
        yield return new WaitForSecondsRealtime(1f);

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        Time.timeScale = 1f; // Chạy lại thời gian
        isTalking = false;

        // 🔥 [QUAN TRỌNG] BÁO ẨN CHUỘT (Vì đã nói chuyện xong)
        if (GameCursorManager.Instance != null)
        {
            GameCursorManager.Instance.isStoryUIOpen = false;
        }

        // Nếu player vẫn trong vùng → hiện lại chữ F
        if (currentNPC != null && currentNPC.PlayerInRange())
        {
            currentNPC.ShowPressF();
        }

        yield return new WaitForSecondsRealtime(1.5f);
        canTalk = true;
    }
}