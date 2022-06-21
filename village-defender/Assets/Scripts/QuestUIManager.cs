using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text questTitle;
    public Text questDescription;
    public Text questGoals;
    public GameObject QuestBox;

    public static QuestUIManager instance;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        questTitle.text = "";
        questDescription.text = "No active Quests";
        questGoals.text = ""; 
    }

    public void ToggleVisible(bool isVisible)
    {
        questTitle.gameObject.SetActive(isVisible);
        questDescription.gameObject.SetActive(isVisible);
        questGoals.gameObject.SetActive(isVisible);
        QuestBox.SetActive(isVisible);
    }

    public void SetQuestTexts(Quest quest)
    {
        questTitle.text = quest.questTitle;
        questDescription.text = quest.questDescription;
        questGoals.text = GetQuestGoals(quest);
    }

    private string GetQuestGoals(Quest quest)
    {
        string goals = "";
        foreach(QuestGoal goal in quest.questGoals)
        {
            if(goal.isDone)
            {
                goals += "[X] " + goal.taskDescription + "\n";
            }
            else
            {
                goals += "[ ] " + goal.taskDescription + "\n";
            }   
        }
        return goals;
    }
}
