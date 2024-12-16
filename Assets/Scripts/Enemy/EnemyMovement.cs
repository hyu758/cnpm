using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidbody2D;

    private List<Vector2> directionList;

    private float timePerUpdateDirection = 0.5f;
    private float timer = 2f;
    public RaycastHit2D lineSign;
    
    private float maxDistanceRaycast = 0.6f;
   
    [FormerlySerializedAs("layerMask")] public int enemyLayerMask = ~(1 << 7);

    [SerializeField] public Vector2 direction = Vector2.up;
    [SerializeField] public float speed;
    
    private List<Vector2> fordable = new List<Vector2>();

    [Header("Sprites")]
    [SerializeField] protected AnimatedSpriteRenderer spriteRendererUp;
    [SerializeField] protected AnimatedSpriteRenderer spriteRendererDown;
    [SerializeField] protected AnimatedSpriteRenderer spriteRendererLeft;
    [SerializeField] protected AnimatedSpriteRenderer spriteRendererRight;
    // public AnimatedSpriteRenderer spriteRendererDeath;
    protected AnimatedSpriteRenderer activeSpriteRenderer;

    [Header("Flicker")]
    [SerializeField] protected float flickerSpeed = 10f;
    [SerializeField] protected Color damageColor = Color.red;
    [SerializeField] protected Color originalColor;
    protected bool isFlickering = false;

    private EnemyType enemyType;
    [SerializeField] public float defaultSpeed;

    private float attackRange;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        activeSpriteRenderer = spriteRendererDown;
        
        defaultSpeed = GetComponent<EnemyStatus>().speedInit;
        speed = defaultSpeed;
        attackRange = GetComponent<EnemyStatus>().attackRange;

        enemyType = GetComponent<EnemyStatus>().enemyType;
    }

    private void Update()
    {
        lineSign = Physics2D.Raycast(transform.position, direction, attackRange, enemyLayerMask);
        
        UpdatePosition();
        UpdateDirection();
    }

    private bool isNeedingToUpdate()
    {
        Vector2 pos = this.transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistanceRaycast, enemyLayerMask);


        if (Vector2.Distance(this.transform.position, pos) <= 0.1f)
        {
            return true;
        }

        if (hit.collider == null)
        {
            return false;
        }


        if (hit.collider.name == "Attacking" || hit.collider.name == "Enemy")
        {
                timer += timePerUpdateDirection;
        }
            
        return true;
    }

    private void UpdateDirection()
    {
        timer += Time.deltaTime;

        if(!isNeedingToUpdate()) return;
        if (IsChasing() || (lineSign.collider?.CompareTag("Bullet") ?? false))
        {
            if (enemyType == EnemyType.Range)
            {
                speed = 0f;
            }
            
            return;
        }

        speed = defaultSpeed;
        if (timer <= timePerUpdateDirection) return;

        fordable = new List<Vector2>();

        if (Physics2D.Raycast(transform.position, Vector2.up, maxDistanceRaycast, enemyLayerMask).collider == null)
        {
            fordable.Add(Vector2.up);
        }
        if (Physics2D.Raycast(transform.position, Vector2.down, maxDistanceRaycast, enemyLayerMask).collider == null)
        { 
            fordable.Add(Vector2.down);
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, maxDistanceRaycast, enemyLayerMask).collider == null)
        {
            fordable.Add(Vector2.left);
        }
        if (Physics2D.Raycast(transform.position, Vector2.right, maxDistanceRaycast, enemyLayerMask).collider == null)
        {
            fordable.Add( Vector2.right);
        }

        // Increase the rate of going straight
        if (Physics2D.Raycast(transform.position, direction, maxDistanceRaycast, enemyLayerMask).collider == null)
        {
            fordable.Add(direction);
            fordable.Add(direction);
            fordable.Add(direction);
            fordable.Add(direction);
        }

        if (fordable.Count == 0) return;

        timer = 0;
        int randomDir = Random.Range(0, fordable.Count);
        direction = fordable[randomDir];

        if (direction == Vector2.up)
            SetDirectionSpriteRenderer(spriteRendererUp);
        else if (direction == Vector2.down)
            SetDirectionSpriteRenderer(spriteRendererDown);
        else if (direction == Vector2.left)
            SetDirectionSpriteRenderer(spriteRendererLeft);
        else if (direction == Vector2.right)
            SetDirectionSpriteRenderer(spriteRendererRight);
    }

    private bool IsChasing()
    {
        if (lineSign.collider == null) return false;
        Debug.Log(lineSign.collider.tag);
        if (lineSign.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    private void UpdatePosition()
    {
        Vector2 pos = _rigidbody2D.transform.position;
        _rigidbody2D.MovePosition(pos + speed * direction * Time.deltaTime);
    }

    private void SetDirectionSpriteRenderer(AnimatedSpriteRenderer spriteRenderer)
    {
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    public void Flickering()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickerCoroutine());
        }
    }

    private IEnumerator FlickerCoroutine()
    {
        isFlickering = true;

        SpriteRenderer objectRenderer1 = spriteRendererUp.GetComponent<SpriteRenderer>();
        SpriteRenderer objectRenderer2 = spriteRendererDown.GetComponent<SpriteRenderer>();
        SpriteRenderer objectRenderer3 = spriteRendererLeft.GetComponent<SpriteRenderer>();
        SpriteRenderer objectRenderer4 = spriteRendererRight.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 5; i++)
        {
            objectRenderer1.color = (objectRenderer1.color == originalColor) ? damageColor : originalColor;
            objectRenderer2.color = (objectRenderer2.color == originalColor) ? damageColor : originalColor;
            objectRenderer3.color = (objectRenderer3.color == originalColor) ? damageColor : originalColor;
            objectRenderer4.color = (objectRenderer4.color == originalColor) ? damageColor : originalColor;

            yield return new WaitForSeconds(1f / flickerSpeed);

            objectRenderer1.color = (objectRenderer1.color == originalColor) ? damageColor : originalColor;
            objectRenderer2.color = (objectRenderer2.color == originalColor) ? damageColor : originalColor;
            objectRenderer3.color = (objectRenderer3.color == originalColor) ? damageColor : originalColor;
            objectRenderer4.color = (objectRenderer4.color == originalColor) ? damageColor : originalColor;

            yield return new WaitForSeconds(1f / flickerSpeed);

        }

        objectRenderer1.color = originalColor;
        objectRenderer2.color = originalColor;
        objectRenderer3.color = originalColor;
        objectRenderer4.color = originalColor;

        isFlickering = false;
    }

}

