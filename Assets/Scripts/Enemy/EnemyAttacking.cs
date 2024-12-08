using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    private int damage;
    private float attackSpeed;
    private EnemyType enemyType;
    [SerializeField] protected CircleCollider2D circleCollider;
    [SerializeField] protected GameObject lazer;
    private RaycastHit2D attackLine;
    private float attackRange;
    private int layerMask;
    private Vector2 direction = Vector2.up;

    protected float timer;

    private void Awake()
    {
        attackSpeed = GetComponentInParent<EnemyStatus>().attackSpeed;
        damage = GetComponentInParent<EnemyStatus>().damage;
        attackRange = GetComponentInParent<EnemyStatus>().attackRange;
        layerMask = GetComponentInParent<EnemyMovement>().enemyLayerMask;
        enemyType = GetComponentInParent<EnemyStatus>().enemyType;
        timer = attackSpeed;
        if (enemyType == EnemyType.Range)
        {
            circleCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (enemyType == EnemyType.Melee)
        {
            return;
        }

        direction = GetComponentInParent<EnemyMovement>().direction;
        attackLine = Physics2D.Raycast(transform.position, direction, attackRange, layerMask);
        RangeAttack();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Attack");
            MeleeAttack(other.GetComponent<PlayerStatus>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = attackSpeed;
        }
    }

    private void MeleeAttack(PlayerStatus _player)
    {
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            timer = 0;
            _player.GetComponent<PlayerStatus>().HandleHurt(damage);
        }
    }

    private void RangeAttack()
    {
        //TODO:for tests
        // if (!IsNeedAttack())
        // {
        //     timer = 0;
        //     return;
        // }
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            timer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject darkExca = Instantiate(lazer, this.transform.position, this.transform.rotation);
        darkExca.transform.SetParent(this.transform.parent);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        darkExca.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        darkExca.transform.position += new Vector3(direction.x, direction.y, 0f);
    }

    private bool IsNeedAttack()
    {
        if (attackLine.collider == null)
        {
            return false;
        }

        if (attackLine.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }
}