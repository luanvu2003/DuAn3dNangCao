using System.Collections.Generic;

public static class DialogueDatabase
{
    public static DialogueNode GetStartNode()
    {
        // ===== KẾT THÚC =====

        DialogueNode acceptEnd = new DialogueNode();
        acceptEnd.text = "Ta tin ngươi. Hãy quay lại khi đã tiêu diệt đủ 5 con quái.";

        DialogueNode refuseEnd = new DialogueNode();
        refuseEnd.text = "Thật đáng tiếc... Hy vọng ta sẽ tìm được người khác.";

        DialogueNode angryEnd = new DialogueNode();
        angryEnd.text = "Ngươi thật vô lễ! Đừng bao giờ quay lại đây nữa!";

        // ===== NHÁNH GIẢI THÍCH =====

        DialogueNode explain2 = new DialogueNode();
        explain2.text = "Chúng xuất hiện từ bóng tối, ban đêm nghe thấy tiếng gào thét khắp khu rừng phía Đông...";

        DialogueNode explain1 = new DialogueNode();
        explain1.text = "Gần đây quái vật xuất hiện rất nhiều. Dân làng sống trong sợ hãi.";

        // ===== NHÁNH HỎI THÊM =====

        DialogueNode askReward = new DialogueNode();
        askReward.text = "Tất nhiên, ta sẽ thưởng cho ngươi 100 vàng và một thanh kiếm nếu ngươi hoàn thành.";

        DialogueNode askDanger = new DialogueNode();
        askDanger.text = "Chúng không phải loại yếu đâu... nhưng ta tin ngươi có thể làm được.";

        // ===== START NODE =====

        DialogueNode start = new DialogueNode();
        start.text = "Chào người lữ hành... Ta đã chờ ngươi từ rất lâu. Ngươi có thể giúp ta một việc không?";

        // ===== GẮN CHOICE =====

        start.choices = new List<DialogueChoice>()
        {
            new DialogueChoice { choiceText = "Có chuyện gì vậy?", nextNode = explain1 },
            new DialogueChoice { choiceText = "Ta không quan tâm.", nextNode = angryEnd }
        };

        explain1.choices = new List<DialogueChoice>()
        {
            new DialogueChoice { choiceText = "Quái vật sao?", nextNode = explain2 },
            new DialogueChoice { choiceText = "Phần thưởng là gì?", nextNode = askReward }
        };

        explain2.choices = new List<DialogueChoice>()
        {
            new DialogueChoice { choiceText = "Nguy hiểm không?", nextNode = askDanger },
            new DialogueChoice { choiceText = "Ta sẽ giúp.", nextNode = acceptEnd }
        };

        askReward.choices = new List<DialogueChoice>()
        {
            new DialogueChoice { choiceText = "Được, ta nhận nhiệm vụ.", nextNode = acceptEnd },
            new DialogueChoice { choiceText = "Thôi bỏ đi.", nextNode = refuseEnd }
        };

        askDanger.choices = new List<DialogueChoice>()
        {
            new DialogueChoice { choiceText = "Ta không sợ.", nextNode = acceptEnd },
            new DialogueChoice { choiceText = "Nghe nguy hiểm quá.", nextNode = refuseEnd }
        };

        acceptEnd.choices = new List<DialogueChoice>();
        refuseEnd.choices = new List<DialogueChoice>();
        angryEnd.choices = new List<DialogueChoice>();

        return start;
    }
}