using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public void ClearSlot(){
        icon.enabled = false;
    }

    public void DisplaySlot(InventoryItem item){
        if(item == null){
            ClearSlot();
            return;
        }
        icon.enabled = true;
        icon.sprite = item.itemData.itemIcon;
    }    
}
