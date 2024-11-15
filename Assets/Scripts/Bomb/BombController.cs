using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{

    [SerializeField] protected GameObject bombPrefab;
    [SerializeField] protected Explosion explosionPrefab;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Tilemap destructibles;
    
    public KeyCode inputKey = KeyCode.Space;

    [SerializeField] protected int bombAmount;
    [SerializeField] protected int bombRemaining;
    [SerializeField] protected float bombFuseTime = 3f;

    [SerializeField] protected int explosionRadius;
    [SerializeField] protected float explosionDuration = 1f;

    private void Awake()
    {
        explosionRadius = PlayerStatus.Instance.RadiusDefault;
        bombAmount = PlayerStatus.Instance.BombAmount;
        bombRemaining = bombAmount;
    }

    void Update()
    {
        PlacingBomb();
    }

    protected void PlacingBomb()
    {
        if (bombRemaining == 0) return;
        if(PlayerStatus.Instance.IsUsingWeapon) return;

        if (Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        GetComponent<PlayerStatus>().PlaceBomb();

        Vector2 pos = this.transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);

        GameObject bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
        bombRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        Destroy(bomb);
        GetComponent<PlayerStatus>().PlusBomb();

        bombRemaining = Mathf.Min(bombRemaining+1, bombAmount);
        SetupExplode(bomb.transform.position);
        
        Destroy(bomb.gameObject);
        
    }

    public void SetupExplode(Vector2 pos)
    {
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);

        Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);

        explosion.SetActiveSpriteRenderer(explosion.top);
        explosion.DestroyAfter(explosionDuration);

        Explode(pos, Vector2.up, explosionRadius);
        Explode(pos, Vector2.down, explosionRadius);
        Explode(pos, Vector2.left, explosionRadius);
        Explode(pos, Vector2.right, explosionRadius);
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) return;
        
        position += direction;
        
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, layerMask))
        {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        if (length > 1)
        {
            explosion.SetActiveSpriteRenderer(explosion.middle);
        }
        else
        {
            explosion.SetActiveSpriteRenderer(explosion.bot);
        }
        
        explosion.SetDirection(direction);
        
        explosion.DestroyAfter(explosionDuration);
        
        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cellPos = destructibles.WorldToCell(position);
        TileBase cell = destructibles.GetTile(cellPos);
    
        if (cell == null) return;
        
        destructibles.SetTile(cellPos,null);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bomb")) {
            other.isTrigger = false;
        }
    }

    public void RadiusChange(int newRadius)
    {
        this.explosionRadius = newRadius;
    }
    
}
