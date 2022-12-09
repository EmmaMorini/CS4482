using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmourCollectible : MonoBehaviour, ICollectible
{
   public static event HandleAmourCollected OnAmourCollected;
    public delegate void HandleAmourCollected(ItemData itemData);
    public ItemData amourData;

    public void Collect()
    {
        Debug.Log("Amour collected");
        Destroy(gameObject);
        OnAmourCollected?.Invoke(amourData);
    }
}
