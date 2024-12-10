using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().HandleHurt(damage);
            Destroy(gameObject);
        }

        if (other.name == "Indestructible")
        {
            Destroy(gameObject);
        }
    }
}
