using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldCollectible : MonoBehaviour, ICollectible
{
    public static event HandleShieldCollected OnShieldCollected;
    public delegate void HandleShieldCollected(ItemData itemData);
    public ItemData shieldData;
    public void Collect()
    {
        Debug.Log("Chalice collected");
        Destroy(gameObject);
        OnShieldCollected?.Invoke(shieldData);
    }
}
