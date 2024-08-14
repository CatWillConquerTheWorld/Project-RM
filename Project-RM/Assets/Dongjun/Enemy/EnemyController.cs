using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //public static EnemyController instance { get; private set; }

    public int hp;
    public int fullHp;

    public bool onGround;

    public float playerRadius;
    public float moveSpeed;

    private bool isGrounded;
    private BoxCollider2D boxCollider;

    private int[] direction;
    public int velocity;
    private bool moved;
    private float wanderTimer;

    public Animator animator;

    public int attackStack;
    public bool isCharging;

    private int normalAttackRate;
    private int semiAttackRate;
    private int chargedAttackRate;

    private bool isHitting;

    private bool isDead;

    //void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    } else
    //    {
    //        Destroy(this);
    //    }
    //}

    void Start()
    {
        FirstSettings();
    }

    void Update()
    {
        if (isDead) return;

        if  (animator.GetCurrentAnimatorStateInfo(0).IsName("attack 1") == false && animator.GetCurrentAnimatorStateInfo(0).IsName("attack 2") == false && isHitting == false && isCharging == false)
        {
            Wander();
        }
    }


    //범위 내에 플레이어가 있는지 확인
    bool PlayerCheck()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= playerRadius)
        {
            return true;
        } 
        else
        {
            return false;
        }
         
    }

    void Wander()
    {
        attackStack = 1;
        float groundCheckAmount = 1f;
        Vector2 groundChecker = boxCollider.bounds.max;
        if (!onGround) groundCheckAmount = 5f;
        if (velocity == -1) groundChecker = boxCollider.bounds.min;
        isGrounded = Physics2D.Raycast(groundChecker, Vector2.down, boxCollider.bounds.extents.y+ groundCheckAmount, LayerMask.GetMask("Ground"));
        wanderTimer -= Time.deltaTime;

        if (wanderTimer < 0f && !moved)
        {
            wanderTimer = Random.Range(1.0f, 2.5f);
            velocity = direction[Random.Range(0,2)];
            moved = true;
        } else if ( wanderTimer < 0f && moved)
        {
            wanderTimer = Random.Range(0.5f, 1.0f);
            velocity = 0;
            moved = false;
        }
        if (!isGrounded)
        {
            velocity *= -1;
            //transform.Translate(Vector3.right * velocity * moveSpeed * 10 * Time.deltaTime);
        }
        transform.Translate(Vector3.right * velocity * moveSpeed * Time.deltaTime);

        if (velocity == 0)
        {
            animator.SetBool("isWalking", false);
        } else if (velocity < 0)
        {
            animator.SetBool("isWalking", true);
            transform.localScale = new Vector3(-3, 3, 0);
        } else if (velocity > 0)
        {
            animator.SetBool("isWalking", true);
            transform.localScale = new Vector3(3, 3, 0);
        }
    }


    public void hitEnd()
    {
        isHitting = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            isHitting = true;
            hp -= normalAttackRate;
            animator.SetTrigger("hit");
        }
        else if (collision.gameObject.name == "SemiChargedBullet(Clone)")
        {
            isHitting = true;
            hp -= semiAttackRate;
            animator.SetTrigger("hit");
        }
        else if (collision.gameObject.name == "ChargedBullet(Clone)")
        {
            isHitting = true;
            hp -= chargedAttackRate;
            animator.SetTrigger("hit");
        }
        if (hp <= 0f)
        {
            Death();
        }
    }

    private void Death()
    {
        print("으앙 쥬금");
        isDead = true;
        animator.SetBool("isDead", true);
        transform.tag = "Untagged";
        Destroy(GetComponent<Rigidbody2D>());
        boxCollider.enabled = false;
    }


    private void FirstSettings()
    {
        fullHp = hp;
        boxCollider = GetComponent<BoxCollider2D>();
        moved = false;
        direction = new int[2] { 1, -1 };
        wanderTimer = 0;
        animator = GetComponent<Animator>();
        isHitting = false;
        isDead = false;
        isCharging = false;

        normalAttackRate = PlayerController.instance.normalAttackRate;
        semiAttackRate = PlayerController.instance.semiAttackRate;
        chargedAttackRate = PlayerController.instance.chargedAttackRate;
    }


    private void OnEnable()
    {
         FirstSettings();
    }
}
