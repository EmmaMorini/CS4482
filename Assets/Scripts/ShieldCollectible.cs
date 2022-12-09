using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldCollectible : MonoBehaviour, ICollectible
{
    public static event Action OnShieldCollected;
    public void Collect()
    {
        Debug.Log("Chalice collected");
        Destroy(gameObject);
        OnShieldCollected?.Invoke();
    }
}
