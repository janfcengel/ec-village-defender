using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceData;

    public GameObject Player;

    public void OnHandlePickupItem()
    {
        InventorySystem.instance.Add(referenceData);
        if (QuestObserver.instance.GetQuest() != null && referenceData.displayName == "Wood")
        {
            QuestObserver.instance.GetQuest().questGoals[1].isDone = true;
            QuestUIManager.instance.SetQuestTexts(QuestObserver.instance.GetQuest());
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            Debug.Log("Player picked up Wood");
            OnHandlePickupItem();
        }
    }

}
