using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Subjects
{
    private static PlayerStatus instance;
    public static PlayerStatus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStatus>();
            }
            return instance;
        }
    }

    private StatusEffectController _statusEffectController;

    [SerializeField] protected bool isUsingWeapon = false;
    public bool IsUsingWeapon => isUsingWeapon;

    [Header("Duration")]
    [SerializeField] protected float durationOfItem = 5f;
    public float DurationOfItem => durationOfItem;
    [SerializeField] protected float durationOfWeapon = 8f;
    public float DurationOfWeapon => durationOfWeapon;

    [Header("Speed")]
    [SerializeField] protected float speedDefault = 5f;
    public float SpeedDefault { get { return speedDefault; } }

    [SerializeField] protected float speedAdded = 3f;
    [SerializeField] protected float speedRealTime;

    [Header("HP")]
    [SerializeField] protected int HP = 10;
    public int CurrentHP => HP;
    [SerializeField] protected int maxHP = 10;
    public int MaxHP => maxHP;

    [Header("Shield")]
    [SerializeField] protected bool hasShield = false;

    [Header("Bomb")]
    [SerializeField] protected int radiusDefault = 2;
    public int RadiusDefault => radiusDefault;
    [SerializeField] protected int radiusRealtime = 3;
    [SerializeField] protected int bombRadiusAdded = 2;

    [SerializeField] protected int bombAmount = 3;
    public int BombAmount => bombAmount;

    private bool isInvulnerable = false;
    [SerializeField] private float invulnerableDuration = 1f;
    
    
    [Header("Item and Weapon Dictionary")]
    [SerializeField] protected Dictionary<Item, float> items = new();
    public Dictionary<Item, float> Items => items;
    [SerializeField] protected Dictionary<Item, int> weapons = new();
    public Dictionary<Item, int> Weapons => weapons;
    
    [Header("References")]
    [SerializeField] protected GameObject spinningAxe;

    [SerializeField] protected GameObject shield;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(this.gameObject);
        _statusEffectController = GetComponent<StatusEffectController>();
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        items = new Dictionary<Item, float>()
        {
            {Item.SpinningAxe, 0},
            {Item.SuperBlastRadius, 0},
            {Item.Shield, 0},
            {Item.SpeedIncrease,0 },
            {Item.Heal, 0},
        };
        weapons = new Dictionary<Item, int>()
        {
            {Item.Excalibur, 0},
            {Item.DarkExcalibur, 0},
        };
    }



    private void Update()
    {
        // UpdateIsUsingWeapon();

        HandlingSpeedUp();
        HandlingShield();
        HandlingBlastRadius();
        HandlingSpinningAxe();

        HandleHeal(2);
        if (HP <= 0)
        {
            this.gameObject.SetActive(false) ;
            
            NotifyObservers(PlayerAction.Lose, 0);
        }
    }

    public void AddItemTime(Item item, int time)
    {
        if (!items.ContainsKey(item))
        {
            Debug.LogError($"Item {item} not found in weapons dictionary.");
            return;
        }
        items[item] = time * durationOfItem;
    }
    public void AddAxeTime(Item item, int time)
    {
        if (!items.ContainsKey(item))
        {
            Debug.LogError($"Item {item} not found in weapons dictionary.");
            return;
        }
        items[item] = time * durationOfWeapon;
    }
    public void AddItemQuantity(Item item, int quality)
    {
        if (!items.ContainsKey(item))
        {
            Debug.LogError($"Item {item} not found in weapons dictionary.");
            return;
        }
        
        items[item] = quality;
    }
    public void AddWeaponQuantity(Item item, int quality)
    {
        if (!weapons.ContainsKey(item))
        {
            Debug.LogError($"Weapon {item} not found in weapons dictionary.");
            return;
        }
        NotifyObservers(PlayerAction.PickUp, 1);
        if (item == Item.Excalibur) PlusExcalibur();
        if (item == Item.DarkExcalibur) PlusDarkExcalibur();
        weapons[item] += quality;
    }
    public void RemoveWeaponQuantity(Item item, int quantity)
    {
        if (!weapons.ContainsKey(item))
        {
            Debug.LogError($"Weapon {item} not found in weapons dictionary.");
            return;
        }
        if(weapons[item] < quantity)
        {
            Debug.Log($"Weapon {item} is not enough to use");
            return;
        }

        if (item == Item.Excalibur)
        {
            Debug.Log(item);
            NotifyObservers(PlayerAction.Excalibur, 0);
        }

        if (item == Item.DarkExcalibur)
        {
            Debug.Log(item);
            NotifyObservers(PlayerAction.DarkExcalibur, 0);
        }
        weapons[item] = Mathf.Max(weapons[item] - quantity, 0);
    }


    public void PlaceBomb()
    {
        NotifyObservers(PlayerAction.PlaceBomb, 1);
    }

    public void PlusBomb()
    {
        NotifyObservers(PlayerAction.PlusBomb, 1);
    }

    public void PlusExcalibur()
    {
        NotifyObservers(PlayerAction.PlusExcalibur, 1);
    }

    public void PlusDarkExcalibur()
    {
        NotifyObservers(PlayerAction.PlusDarkExcalibur, 1);
    }
    public void HandleHurt(int damage)
    {
        if (hasShield || isInvulnerable)
        {
             return;
        }
        
        HP = Mathf.Max(HP-damage, 0);
        _statusEffectController.Flash(Color.red, 4, 0.05f);
        NotifyObservers(PlayerAction.Hurt, damage);
        StartCoroutine(SetInvulnerability());
    }

    private IEnumerator SetInvulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableDuration);
        isInvulnerable = false;
    }
    public void HandleHeal(int bonusHP)
    {
        if (items[Item.Heal] <= 0) return;
        _statusEffectController.Flash(Color.green, 4, 0.05f);
        HP = Mathf.Min(HP + bonusHP, maxHP);
        items[Item.Heal]--;
        Debug.Log($"Heal: {bonusHP}");

        NotifyObservers(PlayerAction.Heal, bonusHP);
    }


    public void HandlingSpeedUp()
    {
        if (items[Item.SpeedIncrease] <= 0)
        {
            PlayerMovement.Instance.ChangeSpeed(speedDefault);
            speedRealTime = speedDefault;

            return;
        }

        items[Item.SpeedIncrease] -= Time.deltaTime;
        if (speedRealTime == speedDefault)
        {
            _statusEffectController.Flash(Color.white, 4, 0.05f);
            speedRealTime = speedDefault + speedAdded;
            PlayerMovement.Instance.ChangeSpeed(speedRealTime);

            NotifyObservers(PlayerAction.SpeedUp, 0);
        }


    }
    public void HandlingShield()
    {
        if (items[Item.Shield] <= 0)
        {
            if (shield.activeSelf) shield.SetActive(false);
            hasShield = false;
            return;
        }
        
        items[Item.Shield] -= Time.deltaTime;
        if (!hasShield)
        {
            if (!shield.activeSelf) shield.SetActive(true);
            hasShield = true;
            NotifyObservers(PlayerAction.Shield, 0);
        }
    }
    public void HandlingBlastRadius()
    {
        if(items[Item.SuperBlastRadius] <= 0)
        {
            GetComponent<BombController>().RadiusChange(radiusDefault);

            radiusRealtime = radiusDefault;
            return;
        }

        items[Item.SuperBlastRadius] -= Time.deltaTime;
        if (radiusRealtime == radiusDefault)
        {
            radiusRealtime = radiusRealtime + bombRadiusAdded;
            GetComponent<BombController>().RadiusChange(radiusRealtime);

            NotifyObservers(PlayerAction.BlastRadius, bombRadiusAdded);
        }
    }
   

    public void HandlingSpinningAxe()
    {
        if (items[Item.SpinningAxe] <= 0)
        {
            if (spinningAxe.activeSelf)
            {
                spinningAxe.SetActive(false);
            }

            return;
        }

        items[Item.SpinningAxe] -= Time.deltaTime;

        if (!spinningAxe.activeSelf)
        {
            spinningAxe.SetActive(true);
        }
    }
    

    // protected void UpdateIsUsingWeapon()
    // {
    //     foreach (var weapon in weapons)
    //     {
    //         if(weapon.Value > 0)
    //         {
    //             isUsingWeapon = true;
    //             return;
    //         }
    //     }
    //     isUsingWeapon = false;
    // }


    public void SetUpForItem(Item itemType)
    {
        
    }
}
