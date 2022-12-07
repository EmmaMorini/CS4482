using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaliceCollectible : MonoBehaviour, ICollectible
{
    public static event Action OnChaliceCollected;

    public void Collect()
    {
        Debug.Log("Chalice collected");
        Destroy(gameObject);
        OnChaliceCollected?.Invoke();
    }
}
