using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(8);

    void OnEnable(){
        Inventory.OnInventoryChange += DisplayInventory;
    }

    void OnDisable(){
        Inventory.OnInventoryChange -= DisplayInventory;
    }

    void ResetInventory(){
        foreach (Transform childTransfrom in transform){
            Destroy(childTransfrom.gameObject);
        }
        inventorySlots = new List<InventorySlot>(8);
    }

    void DisplayInventory(List<InventoryItem> inventory){
        ResetInventory();
        for(int i = 0; i < inventorySlots.Capacity; i++){
            CreateSlot();
        }
        for (int i = 0; i < inventory.Count; i++){
            inventorySlots[i].DisplaySlot(inventory[i]);
        }
    }

    void CreateSlot(){
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.SetParent(transform,false);

        InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
        newSlotComponent.ClearSlot();

        inventorySlots.Add(newSlotComponent);

    }

}
