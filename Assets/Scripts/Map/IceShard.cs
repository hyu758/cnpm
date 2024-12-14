using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceShard : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float timeDelay = 3f;
    [SerializeField] protected float timer = 3f;

    protected bool isPlayerInsideIceShard = false;

    private void Update()
    {
        if (!isPlayerInsideIceShard) return;

        timer += Time.deltaTime; 

        if (timer >= timeDelay)
        {
            DealDamage();
            timer = 0f; 
        }

    }

    private void DealDamage()
    {
        PlayerStatus.Instance.HandleHurt(damage); 
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideIceShard = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideIceShard = false;
            timer = timeDelay;
        }
    }
}
