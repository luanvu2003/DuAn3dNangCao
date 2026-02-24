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

        Time.timeScale = 0f;

        currentNode = DialogueDatabase.GetStartNode();
        ShowNode(currentNode);
    }

    void ShowNode(DialogueNode node)
    {
        npcText.text = node.text;

        // 🔥 Nếu là câu nhận nhiệm vụ thì bật quest
        if (node.text.Contains("tiêu diệt đủ 5"))
        {
            QuestManager.Instance.StartDestroyPillarQuest();
            FindObjectOfType<WorldSpawner>().StartSpawning();
        }

        foreach (Transform child in choicesParent)
            Destroy(child.gameObject);

        if (node.choices == null || node.choices.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }

        foreach (DialogueChoice choice in node.choices)
        {
            GameObject btn = Instantiate(choiceButtonPrefab, choicesParent);

            btn.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                ShowNode(choice.nextNode);
            });
        }
    }

    IEnumerator EndDialogue()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        Time.timeScale = 1f;

        isTalking = false;

        // 🔥 Nếu player vẫn trong vùng → hiện lại F
        if (currentNPC != null && currentNPC.PlayerInRange())
        {
            currentNPC.ShowPressF();
        }

        yield return new WaitForSecondsRealtime(1.5f);
        canTalk = true;
    }
}