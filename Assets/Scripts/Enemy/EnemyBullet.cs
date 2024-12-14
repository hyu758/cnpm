using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        
        if (!other.CompareTag("Enemy"))
        {
            SelfDestroy();
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().HandleHurt(damage);
        }
    }
}
