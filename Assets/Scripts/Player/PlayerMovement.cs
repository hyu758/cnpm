using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
            }
            return instance;
        }
    }

    protected Rigidbody2D rb;
    protected float speed;
    [SerializeField] protected bool isMoving = false;
    

    [Header("Input")]
    [SerializeField] protected Color damageColor = Color.red;
    protected Color originalColor;
    
    private Animator animator;
    private PlayerInputAction playerControls;
    private Vector2 direction;
    public Vector2 Direction => direction;
    private Vector2 signDirection;
    public Vector2 SignDirection => signDirection;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(this.gameObject);

        playerControls = new PlayerInputAction();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = PlayerStatus.Instance.SpeedDefault;
        originalColor = Color.white;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        PlayerInput();
    }
    

    private void PlayerInput()
    {
        if (playerControls.Movement.Move.ReadValue<Vector2>().normalized != Vector2.zero)
        {
            direction = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        animator.SetFloat("XDir", direction.x);
        animator.SetFloat("YDir", direction.y);

        if (isMoving)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
        
    }

    private void Move()
    {
        if (!isMoving)
        {
            return;
        }
        rb.MovePosition(rb.position + direction * (speed * Time.fixedDeltaTime));
    }
    public void ChangeSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }
    
    
    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

}