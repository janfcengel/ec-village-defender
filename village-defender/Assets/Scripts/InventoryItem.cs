using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem 
{
    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set;  }

    public InventoryItem(InventoryItemData data)
    {
        this.data = data;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}
