using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public bool isComplete; 
    public string questTitle;
    public string questDescription;
    public List<QuestGoal> questGoals; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isQuestComplete()
    {
        return isComplete; 
    }
}
