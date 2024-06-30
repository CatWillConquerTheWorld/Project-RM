using System.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 4f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private BoxCollider2D boxCollider;

    //총 요소
    public GameObject bulletPrefab;
    public GameObject chargedBulletPrefab;
    public GameObject semiChargedBP;
    public Transform firePoint;
    public float bulletSpeed;
    public float chargedBulletSpeed;
    public float maxChargeTime;

    public Animator gunAnimator;
    public Animator gunVFXAnimator;
    public SpriteRenderer gunSpriteRenderer;
    public SpriteRenderer gunVFXSpriteRenderer;
    private float chargedTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        //좌우 이동
        Move();

        // 점프
        Jump();

        // 총 발사
        Shoot();

        //시야 위/아래로 전환
        CameraControl();
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

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = true;
            gunSpriteRenderer.flipX=true;
            gunVFXSpriteRenderer.flipX=true;
            transform.Translate(Vector3.right * -1 * moveSpeed * Time.deltaTime);

            // 총 위치 조정
            transform.Find("Gun").transform.position = transform.position + new Vector3(0.1f, -0.275f, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = false;
            gunSpriteRenderer.flipX = false;
            gunVFXSpriteRenderer.flipX = false;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            //총 위치 조정
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
        print(chargedTime);
        if (Input.GetKeyDown(KeyCode.A))
        {
            chargedTime = 0;
            gunAnimator.SetBool("isCharging", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            chargedTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.A))
         {
            if (chargedTime >= maxChargeTime)
            {
                GunEffect("Charged");
                gunAnimator.SetBool("isCharging", false);
                Attack("Charged");
            } 
            else if (chargedTime >= maxChargeTime / 3 * 2)
            {
                GunEffect("Charged");
                gunAnimator.SetBool("isCharging", false);
                Attack("SemiCharged");
            }
            else
            {
                GunEffect("Normal");
                gunAnimator.SetBool("isCharging", false);
                Attack("Normal");
            }
        }
    }

    private void GunEffect(string type)
    {
        gunVFXAnimator.StopPlayback();
        if (gunVFXAnimator.GetCurrentAnimatorStateInfo(0).IsName("charged") == true || gunVFXAnimator.GetCurrentAnimatorStateInfo(0).IsName("normal") == true)
        {
            gunVFXAnimator.SetTrigger("back");
        }
        if (type == "Charged")
        {
            gunVFXAnimator.SetTrigger("ChargedShoot");
        } 
        else if (type == "Normal")
        {
            gunVFXAnimator.SetTrigger("Shoot");
        }
    }

    private void Attack(string type)
    {
        if (type == "Charged")
        {
            GameObject chargedBullet = Instantiate(chargedBulletPrefab, firePoint.position, firePoint.rotation);
            chargedBullet.gameObject.SetActive(true);
            Rigidbody2D rb = chargedBullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * chargedBulletSpeed;
            chargedBullet.GetComponent<Animator>().SetTrigger("charged");
        } 
        else if (type == "SemiCharged")
        {
            GameObject semiChargedBullet = Instantiate(semiChargedBP, firePoint.position, firePoint.rotation);
            semiChargedBullet.gameObject.SetActive(true);
            Rigidbody2D rb = semiChargedBullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * chargedBulletSpeed;
            semiChargedBullet.GetComponent<Animator>().SetTrigger("semi");
        }
        else if (type == "Normal")
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.gameObject.SetActive(true);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * bulletSpeed;
            bullet.GetComponent<Animator>().SetTrigger("normal");
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