using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Subjects
{

    public int HP;
    public float speedInit;
    public int damage;
    public float attackSpeed;
    public float attackRange;
    public EnemyType enemyType;

    private void Update()
    {
        if (HP <= 0) 
        {
            this.gameObject.SetActive(false);
            
        }
    }

    public void HandleHurt(int damage)
    {
        HP -= damage;
        GetComponent<EnemyMovement>().Flickering();
    }
    
}
