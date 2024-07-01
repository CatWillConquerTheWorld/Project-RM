using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float playerRadius;
    public float moveSpeed;

    private bool isGrounded;
    private BoxCollider2D boxCollider;

    private int[] direction;
    private int velocity;
    private bool moved;
    private float wanderTimer;

    private Animator animator;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        moved = false;
        direction = new int[2] { 1, -1 };
        wanderTimer = 0;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerCheck())
        {

        } else
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
}
