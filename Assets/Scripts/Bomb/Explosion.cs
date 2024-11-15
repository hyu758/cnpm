using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public AnimatedSpriteRenderer top;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer bot;
    
    // Start is called before the first frame update
    public void SetActiveSpriteRenderer(AnimatedSpriteRenderer renderer)
    {
        top.enabled = renderer == top;
        middle.enabled = renderer == middle;
        bot.enabled = renderer == bot;
    }
    
    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        Debug.Log(other.tag + " take hit by bomb!");

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().HandleHurt(1);
        }
        
    }
}
