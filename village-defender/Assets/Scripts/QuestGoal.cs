using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public bool isDone;

    public string taskDescription;

    public int amountForQuestGoal;
    public int currentForQuestGoal;

    public QuestType questType;

    public bool isQuestGoalComplete()
    {
        return isDone;
    }

}

public enum QuestType
{
    Enter,
    Collect,
}
