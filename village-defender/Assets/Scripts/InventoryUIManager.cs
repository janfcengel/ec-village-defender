using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text inventoryTitle;
    public Text inventoryItems;
    public GameObject InventoryBox;

    public static InventoryUIManager instance;
    public InventorySystem inv; 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventoryTitle.text = "";
        inventoryItems.text = "No items yet";
    }

    private void Update()
    {
        SetItemTexts();
    }

    public void ToggleVisible(bool isVisible)
    {
        inventoryTitle.gameObject.SetActive(isVisible);
        inventoryItems.gameObject.SetActive(isVisible);
        InventoryBox.SetActive(isVisible);
    }

    public void SetItemTexts()
    {
        string itemsText = "";
        if(inv.inventory.Count == 0)
        {
            inventoryItems.text = "No items yet";
            return; 
        }
        foreach(InventoryItem item in inv.inventory)
        {
            itemsText += item.stackSize + "x " + item.data.displayName;
        }
        inventoryTitle.text = "Inventory";
        inventoryItems.text = itemsText;
    }
}
