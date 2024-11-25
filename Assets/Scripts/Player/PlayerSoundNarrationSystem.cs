using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundNarrationSystem : MonoBehaviour, IObserver
{
    public Subjects _playerSubject;

    private Dictionary<PlayerAction, System.Action> _playerActionHandler;

    private void Awake()
    {
        _playerActionHandler = new Dictionary<PlayerAction, Action>()
        {
            { PlayerAction.Hurt, HandleHurt },
            { PlayerAction.SpeedUp, HandleSpeedIncrease },
            { PlayerAction.Shield, HandleShield },
            { PlayerAction.BlastRadius, HandleBlastRadius },
            { PlayerAction.HandleBomb, HandleHandleBomb },
            { PlayerAction.Heal, HandleHeal},
            { PlayerAction.PlaceBomb, PlaceBomb }
        };
    }

    public void OnNotify(PlayerAction action, float n)
    {
        if (_playerActionHandler.ContainsKey(action))
        {
            _playerActionHandler[action]();
        }
    }
    
    public void OnNotify(BossAction action, float n)
    {
        
    }

    private void PlaceBomb()
    {
        Debug.Log("Dat bom ne");
    }

   
    private void HandleHurt()
    {
        
    }

    private void HandleShield()
    {
        
    }

    private void HandleHandleBomb()
    {
        
    }

    private void HandleSpeedIncrease()
    {
        
    }

    private void HandleBlastRadius()
    {
        
    }

    private void HandleHeal()
    {
        Debug.Log("Jett heal me");
    }

    private void OnEnable()
    {
        _playerSubject.AddObservoer(this);
    }

    protected void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }
}
