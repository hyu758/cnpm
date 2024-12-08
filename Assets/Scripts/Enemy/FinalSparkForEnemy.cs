using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSparkForEnemy : FinalSpark
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().HandleHurt(damage);
        }
        
    }
}
