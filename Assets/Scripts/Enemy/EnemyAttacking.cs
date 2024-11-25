using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    
    [SerializeField] protected int damage;
    [SerializeField] protected float attackSpeed;
    protected float timer;

    private void Awake()
    {
        attackSpeed = GetComponentInParent<EnemyStatus>().attackSpeed;
        damage = GetComponentInParent<EnemyStatus>().damage;
        timer = attackSpeed;
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Attack");
            Attack(other.GetComponent<PlayerStatus>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = attackSpeed;
        }
    }

    private void Attack(PlayerStatus _player)
    {
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            timer = 0;
            _player.GetComponent<PlayerStatus>().HandleHurt(damage);
        }
    }
}
