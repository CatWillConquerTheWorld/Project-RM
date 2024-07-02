using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int hp;

    public float playerRadius;
    public float moveSpeed;

    private bool isGrounded;
    private BoxCollider2D boxCollider;

    private int[] direction;
    private int velocity;
    private int attackVelo;
    private bool moved;
    private float wanderTimer;

    private Animator animator;

    private bool isCharging;
    private int attackStack;
    private GameObject attackCollider;
    private GameObject longAttackCollider;
    private int normalAttackRate;
    private int semiAttackRate;
    private int chargedAttackRate;

    private bool isHitting;

    private bool isDead;

    void Start()
    {
        attackStack = 1;
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

        attackCollider = transform.Find("attackCollider").gameObject;
        attackCollider.SetActive(false);
        longAttackCollider = transform.Find("longAttackCollider").gameObject;
        longAttackCollider.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            print("S");
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            LongAttackPrepare();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            LongAttack();
        }

        if  (animator.GetCurrentAnimatorStateInfo(0).IsName("attack 1 0") == false && animator.GetCurrentAnimatorStateInfo(0).IsName("attack 2 0") == false && isHitting == false && isCharging == false)
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
        isGrounded = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y+ 0.1f, LayerMask.GetMask("Ground"));
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
            transform.Translate(Vector3.right * velocity * moveSpeed * 10 * Time.deltaTime);
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

    void Attack()
    {
        attackCollider.SetActive(false);
        velocity = 0;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction.x < 0f)
        {
            
            transform.localScale = new Vector3(-3, 3, 0);
        }
        else
        {
            transform.localScale = new Vector3(3, 3, 0);
        }
        attackCollider.SetActive(true);
        if (attackStack % 2 == 1) animator.SetTrigger("attack1");
        if (attackStack % 2 == 0) animator.SetTrigger("attack2");
    }

    void LongAttackPrepare()
    {
        longAttackCollider.SetActive(false);
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction.x < 0f)
        {
            attackVelo = -1;
            transform.localScale = new Vector3(-3, 3, 0);
        }
        else
        {
            attackVelo = 1;
            transform.localScale = new Vector3(3, 3, 0);
        }
        isCharging = true;
        animator.SetBool("isCharging", true);
    }

    void LongAttack()
    {
        longAttackCollider.SetActive(true);
        animator.SetTrigger("longAttack");
    }

    public void attackStackPlus()
    {
        attackStack += 1;
    }

    public void back()
    {
        isCharging = false;
        animator.SetBool("isCharging", false);
        attackCollider.SetActive(false);
        longAttackCollider.SetActive(false);
        animator.SetTrigger("back");
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

}
