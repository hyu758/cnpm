using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FinalSpark : MonoBehaviour
{

    public int damage;
    [SerializeField] protected int size = 1;
    [SerializeField] protected float waitingTime = 2f;
    [SerializeField] protected float shootingTime = 3f;
    protected Collider2D col;
    protected Animator animator;

    private Tilemap destructibles;
    [SerializeField] protected GameObject explodingPrefab;

    protected int limitXMin = 0;
    protected int limitXMax = 0;
    protected int limitYMin = 0;
    protected int limitYMax = 0;
    protected float rotationAngles = 0;

    public bool IsShooting
    {
        get { return animator.GetBool(AnimationString.isShooting); }
        set
        {
            animator.SetBool(AnimationString.isShooting, value);
        }
    }


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        destructibles = GameObject.Find("Destructible").GetComponent<Tilemap>();
    }

    private void Start()
    {
        IsShooting = false;
        StartCoroutine(Shoot());
    }
    private void Update()
    {
        rotationAngles = transform.rotation.eulerAngles.z; 
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(waitingTime);

        IsShooting = true;
      
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 5f * Time.deltaTime);
        
        if(destructibles) ClearDestructibles();

        yield return new WaitForSeconds(shootingTime);

        IsShooting = false; 

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
    private void ClearDestructibles()
    {
        Vector2 center = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        SetLimit();

        for (int i = limitXMin; i <= limitXMax; i++)
        {
            for (int j = limitYMin; j <= limitYMax; j++)
            {
                Vector2 tmpPos = new Vector2(center.x + i, center.y + j);
                if (!IsPositionInsideTilemap(tmpPos)) continue;
                ClearDestructible(tmpPos);
            }
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

    private void SetLimit() 
    {
        Debug.Log(rotationAngles);
        if (rotationAngles == 90)
        {
            limitXMin = -size / 2;
            limitXMax = size / 2;
            limitYMin = 0;
            limitYMax = 20;
        }
        else if (rotationAngles == 270)
        {
            limitXMin = -size / 2;
            limitXMax = size / 2;
            limitYMin = -20;
            limitYMax = 0;
        }
        else if (rotationAngles == 0)
        {
            limitXMin = 0;
            limitXMax = 20;
            limitYMin = -size / 2;
            limitYMax = size / 2;
        }
        else
        {
            limitXMin = -20;
            limitXMax = 0;
            limitYMin = -size / 2;
            limitYMax = size / 2;
        }
    }

    bool IsPositionInsideTilemap(Vector3 worldPosition)
    {
        BoundsInt bounds = destructibles.cellBounds;
        Vector3Int cellPosition = destructibles.WorldToCell(worldPosition);
        return bounds.Contains(cellPosition);
    }

}
