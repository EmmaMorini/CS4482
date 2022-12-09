using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public static event Action<List<InventoryItem>> OnInventoryChange;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDict = new Dictionary<ItemData, InventoryItem>();

    void OnEnable(){
        AmourCollectible.OnAmourCollected += Add;
    }

    void OnDisable(){
         AmourCollectible.OnAmourCollected -= Add;
    }

    public void Add(ItemData itemData)
    {
        if (itemDict.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            Debug.Log($"{item.itemData.itemName} has {item.stackSize}");
            OnInventoryChange?.Invoke(inventory);
        }
        else{
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDict.Add(itemData,newItem);
            Debug.Log($"Added {itemData.itemName} first time");
            OnInventoryChange?.Invoke(inventory);
        }
    }

    public void Remove(ItemData itemData){
        if (itemDict.TryGetValue(itemData,out InventoryItem item)){
            item.RemoveFromStack();
            if(item.stackSize == 0){
                inventory.Remove(item);
                itemDict.Remove(itemData);
            }
            OnInventoryChange?.Invoke(inventory);
        }
    }
}
