using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemType;
    [SerializeField] protected float destroyAfter = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    public void OnPickupItem(PlayerStatus other)
    {
        switch (itemType)
        {
            case Item.Shield:
                other.AddItemTime(Item.Shield, 1);
                break;
            case Item.SpeedIncrease:
                other.AddItemTime(Item.SpeedIncrease, 1);
                break;
            case Item.SuperBlastRadius:
                other.AddItemTime(Item.SuperBlastRadius, 1);
                break;
            case Item.Heal:
                other.AddItemQuantity(Item.Heal, 1);
                break;
            case Item.SpinningAxe:
                other.AddAxeTime(Item.SpinningAxe, 1);
                break;
            case Item.Excalibur:
                other.AddWeaponQuantity(Item.Excalibur, 1);
                PlayerStatus.Instance.SetUpForItem(itemType);
                break;
            case Item.DarkExcalibur:
                other.AddWeaponQuantity(Item.DarkExcalibur, 1);
                PlayerStatus.Instance.SetUpForItem(itemType);
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickupItem(other.gameObject.GetComponent<PlayerStatus>());
        }
    }
}
