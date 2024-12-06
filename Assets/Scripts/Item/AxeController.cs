using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AxeController : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    [SerializeField] protected int size = 1;

    private Tilemap destructibles;
    [SerializeField] protected GameObject explodingPrefab;

    private void Awake()
    {
        destructibles = GameObject.Find("Destructible").GetComponent<Tilemap>();
    }
    void Update()
    {
        if (destructibles)
        {
            ClearDestructibles();
        }
    }

    private void ClearDestructibles()
    {
        Vector2 center = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        for (int i = -size / 2; i <= size / 2; i++)
        {
            ClearDestructible(new Vector2(center.x - i, center.y));
            ClearDestructible(new Vector2(center.x, center.y - i));
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        //Debug.Log(other.name);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStatus>().HandleHurt(damage);
        }
        
    }
}
