using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSparkForPlayer : FinalSpark
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStatus>().HandleHurt(damage);
        }
        
    }
}
