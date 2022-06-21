using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObserver : MonoBehaviour
{
    public PlayerController playerController;
    public InventorySystem inv;

    public static QuestObserver instance;

    public InventoryItemData wood;
    public InventoryItemData axe;
    private void Awake()
    {
        instance = this; 
    }

    private void Update()
    {
        if(GetQuest() == null) { return; }

        if(GetQuest().isQuestComplete() && GetQuest().delFlag == false)
        {
            inv.Add(axe);
            inv.Remove(wood);
            GetQuest().delFlag =true;
        }
    }
    public Quest GetQuest()
    {
        return playerController.GetQuest(); 
    }
}
