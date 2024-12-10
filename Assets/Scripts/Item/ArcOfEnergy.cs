using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArcOfEnergy : Bullet
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStatus>().HandleHurt(damage);
        }
        if (other.CompareTag("Boss"))
        {
            other.GetComponent<BossController>().HandleHurt(damage);
        }
    }
}
