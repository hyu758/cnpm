using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINarrationSystemForBossMap : MonoBehaviour, IObserver
{
    [Header ("Player")]
    public Image healthBar;
    public int playerMaxHP = 10;
    public int playerHP;
    public bool isWin = false;

    [Header("Boss")]
    public Image bossHealthBar;
    public int bossMaxHP = 100;
    public int bossHP;

    [Header("Bomb")]
    public int bombAmount;
    public int currentBomb;
    public Text bombNumber;

    public int radiusDefault;
    public int currentRadius;
    public Text radiusText;

    [Header("Weapon quantity")]
    public int excaliburAmount;
    public int darkExcaliburAmount;
    public TMP_Text excaliburText;
    public TMP_Text darkExcaliburText;
    
    [Header("Shield and Speed")]
    public GameObject shield;
    public GameObject speedUp;


    [Header("UI endgame")]
    public GameObject endGameInfo;
    public Text statement;
    public Image endBg;

    [Header("References")]
    public Subjects _playerSubject;
    public PlayerStatus _playerStatus;
    public Subjects _levelManager;
    public Subjects _bossController;

    private Dictionary<PlayerAction, System.Action<float>> _playerActionHandler;
    private Dictionary<BossAction, System.Action<float>> _bossActionHandler;

    private void Awake()
    { 
        radiusDefault = PlayerStatus.Instance.RadiusDefault;
        currentRadius = radiusDefault;
        bombAmount = PlayerStatus.Instance.BombAmount;
        currentBomb = bombAmount;

        playerMaxHP = PlayerStatus.Instance.MaxHP;
        playerHP = PlayerStatus.Instance.CurrentHP;

        bossMaxHP = _bossController.GetComponent<BossController>().maxHP;
        bossHP = _bossController.gameObject.GetComponent<BossController>().HP;

        bombNumber.text = currentBomb.ToString();
        radiusText.text = currentRadius.ToString();

        SetUpDictionaryAction();
    }

    public void SetUpDictionaryAction()
    {
        _playerActionHandler = new Dictionary<PlayerAction, Action<float>>()
        {
            { PlayerAction.Hurt,(n) => HandleHurt(n) },
            { PlayerAction.SpeedUp,(n) => HandleSpeedUp(n) },
            { PlayerAction.Shield,(n) => HandleShield(n) },
            { PlayerAction.BlastRadius,(n)=> HandleBlastRadius(n) },
            { PlayerAction.Excalibur, (n) => Excalibur(n)},
            { PlayerAction.DarkExcalibur, (n) => DarkExcalibur(n)},
            { PlayerAction.Heal, (n)=> HandleHeal(n)},
            { PlayerAction.PlaceBomb, (n) =>PlaceBomb(n)},
            { PlayerAction.PlusBomb, (n) =>PlusBomb(n)},
            { PlayerAction.Win, (n) => Win(n) },
            { PlayerAction.Lose, (n) => Lose(n) },
            { PlayerAction.PlusExcalibur, (n) => PlusExcalibur(n)},
            { PlayerAction.PlusDarkExcalibur, (n) => PlusDarkExcalibur(n)}
        };

        _bossActionHandler = new Dictionary<BossAction, Action<float>>()
        {
            { BossAction.Hurt,(n) => BossHandleHurt(n) },
            //{ BossAction.Angry,(n)=>  }
        };
    }

    public void ReloadSceneBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HomeBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void Win(float n)
    {
        if (!endGameInfo) return;
        if (!isWin) return;
        Debug.Log("Win");
        statement.text = "You Win";
        statement.color = Color.white;
        endGameInfo.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Lose(float n)
    {
        if (!endGameInfo) return;
        Debug.Log("Lose");
        statement.text = "You Lose";
        statement.color = new Color(255f / 255f, 0f, 61f / 255f);
        endGameInfo.SetActive(true);
        Time.timeScale = 0f;

    }

    public void OnNotify(PlayerAction action, float n)
    {
        if (_playerActionHandler.ContainsKey(action))
        {
            _playerActionHandler[action](n);
        }
    }
    
    public void OnNotify(BossAction action, float n)
    {
        if (_bossActionHandler.ContainsKey(action))
        {
            _bossActionHandler[action](n);
        }
    }

    private void HandleHurt(float n)
    {
        playerHP = (int) Mathf.Max(playerHP - n, 0);

        healthBar.fillAmount = playerHP/ ((float)1.0 * playerMaxHP);

        Debug.Log("- mau o thanh HP ne");
    }

    private void BossHandleHurt(float n)
    {
        bossHP = (int)Mathf.Max(bossHP - n, 0);

        bossHealthBar.fillAmount = bossHP / ((float)1.0 * bossMaxHP);

        // Debug.Log("- mau boss");
    }

    private void HandleHeal(float n)
    {
        playerHP = (int)Mathf.Min(playerHP + n, playerMaxHP);

        healthBar.fillAmount = playerHP / ((float)1.0 * playerMaxHP);
         Debug.Log("+ mau o thanh HP ne");
    }

    private void PlaceBomb(float n)
    {
        currentBomb -= 1;
        bombNumber.text = currentBomb.ToString();
        // Debug.Log("-1 o thanh bomb Amount ne");
    }

    private void PlusBomb(float n)
    {
        currentBomb = Mathf.Min(currentBomb + 1, bombAmount);
        bombNumber.text = currentBomb.ToString();
        // Debug.Log("+1 o thanh Bomb Amount ne");
    }

    private void HandleSpeedUp(float n)
    {
        StartCoroutine(SpeedUpOn(n));
    }
    private void HandleShield(float n)
    {
        StartCoroutine(ShieldOn(n));
    }


    private void HandleBlastRadius(float n)
    {
        StartCoroutine(ChangeBlastRadius(n));
        Debug.Log("+ o radius ne");
    }

    private void PlusExcalibur(float n)
    {
        excaliburAmount++;
        Debug.Log(excaliburAmount);
        excaliburText.text = excaliburAmount.ToString();
    }

    private void PlusDarkExcalibur(float n)
    {
        darkExcaliburAmount++;
        Debug.Log(darkExcaliburAmount);
        darkExcaliburText.text = darkExcaliburAmount.ToString();
    }

    private void Excalibur(float n)
    {
        excaliburAmount--;
        excaliburText.text = excaliburAmount.ToString();
    }

    private void DarkExcalibur(float n)
    {
        darkExcaliburAmount--;
        darkExcaliburText.text = darkExcaliburAmount.ToString();
    }
    private IEnumerator SpeedUpOn(float n)
    {
        speedUp.SetActive(true);

        yield return new WaitForSeconds(PlayerStatus.Instance.DurationOfItem);

        speedUp.SetActive(false);
    }
    private IEnumerator ShieldOn(float n)
    {
        shield.SetActive(true);

        yield return new WaitForSeconds(PlayerStatus.Instance.DurationOfItem);

        shield.SetActive(false);
    }

    private IEnumerator ChangeBlastRadius(float n)
    {
        currentRadius += (int) n;
        radiusText.text = currentRadius.ToString();

        yield return new WaitForSeconds(PlayerStatus.Instance.DurationOfItem);
        currentRadius = radiusDefault;
        radiusText.text = currentRadius.ToString();
    }

    private void OnEnable()
    {
        _playerSubject.AddObservoer(this);
        _levelManager.AddObservoer(this);
        _bossController.AddObservoer(this);
    }

    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
        _levelManager.RemoveObserver(this);
        _bossController.RemoveObserver(this);   
    }
}
