﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] protected float speed = 10;
    [SerializeField] protected int size = 1;

    protected Tilemap destructibles;
    [SerializeField] protected GameObject explodingPrefab;

    private void Start()
    {
        destructibles = GameObject.Find("Destructible").GetComponent<Tilemap>();
        // destructibles = new Tilemap();
    }
    
    private void Update()
    {
        BulletFlying();
        
        Vector3Int cellPosition = destructibles.WorldToCell(this.transform.position);

        if (destructibles.GetTile(cellPosition))
        {
            Debug.Log(destructibles);
            ClearDestructibles();
            SelfDestroy();
        }
    }

    protected void BulletFlying()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    protected void ClearDestructibles()
    {
        Vector2 center = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        for (int i = -size/2; i <= size/2; i++)
        {
            ClearDestructible(new Vector2(center.x + i, center.y));
            ClearDestructible(new Vector2(center.x, center.y + i));
        }
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cellPos = destructibles.WorldToCell(position);
        TileBase cell = destructibles.GetTile(cellPos);

        if (cell == null) return;

        Instantiate(explodingPrefab, position, Quaternion.identity);
        destructibles.SetTile(cellPos, null);
    }

    protected void SelfDestroy()
    {
        Debug.Log("asdasdasdasd");
        Destroy(this.GameObject());
    }
}
