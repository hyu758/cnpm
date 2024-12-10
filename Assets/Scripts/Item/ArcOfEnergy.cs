using UnityEngine;

public class ArcOfEnergy : Bullet
{
    void Update()
    {
        BulletFlying(); 
        Vector3Int cellPosition = destructibles.WorldToCell(this.transform.position);

        if (destructibles.GetTile(cellPosition))
        {
            ClearDestructibles();
        }
    }
    
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
