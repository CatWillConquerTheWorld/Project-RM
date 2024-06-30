using UnityEngine;

public class Assasin : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 4f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private BoxCollider2D boxCollider;

    //�� ���
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;

    private Animator gunAnimator;
    private SpriteRenderer gunSpriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunAnimator = transform.Find("Gun").GetComponent<Animator>();
        gunSpriteRenderer = transform.Find("Gun").GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        //�¿� �̵�
        Move();

        // ����
        Jump();

        // �� �߻�
        Shoot();

        //�þ� ��/�Ʒ��� ��ȯ
        CameraControl();
    }

    // �±׸� ���Ͽ� isGrounded �� �ִϸ������� ������ �ٷ�� ���, �̴� �������� ������ �Ǵ� ���װ� �־� RayCast ä��
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
        // �� üũ
        isGrounded = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = true;
            gunSpriteRenderer.flipX=true;
            transform.Translate(Vector3.right * -1 * moveSpeed * Time.deltaTime);

            // �� ��ġ ����
            transform.Find("Gun").transform.position = transform.position + new Vector3(0.1f, -0.275f, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = false;
            gunSpriteRenderer.flipX = false;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            //�� ��ġ ����
            transform.Find("Gun").transform.position = transform.position + new Vector3(-0.1f, -0.275f, 0);
        }
        else
        {
            animator.SetBool("isWalking", false);
            //animator.StopPlayback();
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            //rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

        if (!isGrounded)
        {
            animator.SetBool("isJump", true);
        }
        else
        {
            animator.SetBool("isJump", false);
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") == true)
            {
                gunAnimator.SetTrigger("back");
            }
            gunAnimator.SetTrigger("Shoot");
            //Instantiate(bullet, firePoint.position, firePoint.rotation);

            // firePoint ��ġ���� bulletPrefab�� �ν��Ͻ�ȭ
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.gameObject.SetActive(true);
            // �Ѿ��� Rigidbody2D�� �����ͼ� �ӵ��� ����
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * bulletSpeed;
        }
    }

    private void CameraControl()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            CameraController.instance.offset = Vector3.Lerp(CameraController.instance.offset, new Vector3(0, 2, 0), 0.05f);
        } else if (Input.GetKey(KeyCode.Alpha2))
        {
            CameraController.instance.offset = Vector3.Lerp(CameraController.instance.offset, new Vector3(0, -2, 0), 0.05f);
        } else
        {
            CameraController.instance.offset = Vector3.Lerp(CameraController.instance.offset, Vector3.zero, 0.05f);
        }
    }
}