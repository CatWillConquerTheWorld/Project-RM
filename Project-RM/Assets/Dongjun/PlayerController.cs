using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 4f;
    public GameObject bullet;
    public Transform firePoint;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private BoxCollider2D boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        print(isGrounded);
        // 좌우 이동
        //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 걷는 모션 설정
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = true;
            transform.Translate(Vector3.right * -1 * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = false;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
            //animator.StopPlayback();
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            //rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

        if (!isGrounded)
        {
            animator.SetBool("isJump", true);
        } else
        {
            animator.SetBool("isJump", false);
        }

        // 총 발사
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("Attack");
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

    // 태그를 비교하여 isGrounded 및 애니메이터의 변수를 다루는 방법, 이는 벽에서도 점프가 되는 버그가 있어 RayCast 채용
    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //        animator.SetBool("isJump", false);
    //    }
    //}

    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //        animator.SetBool("isJump", true);
    //    }
    //}

    void FixedUpdate()
    {
        // 땅 체크
        isGrounded = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
    }
}