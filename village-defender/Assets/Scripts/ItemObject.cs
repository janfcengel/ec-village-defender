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
