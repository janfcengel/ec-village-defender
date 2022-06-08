using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem: MonoBehaviour
{
    public List<InventoryItem> inventory;
    public Dictionary<InventoryItemData, InventoryItem> itemDictionary;

    public static InventorySystem instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if(itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null; 
    }

    public void Add(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(referenceData);
            }
        }
    }
}
