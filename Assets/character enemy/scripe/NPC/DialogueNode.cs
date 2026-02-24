using System.Collections.Generic;

[System.Serializable]
public class DialogueNode
{
    public string text;
    public List<DialogueChoice> choices;

    public bool startQuest = false; // 🔥 thêm dòng này thôi
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
}