using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Exploding : MonoBehaviour
{
    public float itemDropRate = 1f;
    public List<ItemPickupWithRate> itemPickups;

    void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        if (itemPickups.Count == 0) return;

        float randomValue = Random.Range(0f, 1f);
        Debug.Log(randomValue);
        if (randomValue > itemDropRate)
        {
            Debug.Log("Box drop nothing");
            return;
        }

        ItemPickup selectedItem = GetRandomItem();
        if (selectedItem != null)
        {
            Instantiate(selectedItem, transform.position, Quaternion.identity);
        }
    }

    private ItemPickup GetRandomItem()
    {
        int totalRate = 0;
        foreach (var item in itemPickups)
        {
            totalRate += item.rate;
        }

        int randomValue = Random.Range(0, totalRate);
        int cumulativeRate = 0;

        foreach (var item in itemPickups)
        {
            cumulativeRate += item.rate;
            if (randomValue < cumulativeRate)
            {
                return item.item;
            }
        }

        return null; 
    }
}
