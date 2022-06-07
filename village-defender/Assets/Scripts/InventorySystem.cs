using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem: MonoBehaviour
{
    public List<InventoryItem> inventory;
    public Dictionary<InventoryItemData, InventoryItem> itemDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
