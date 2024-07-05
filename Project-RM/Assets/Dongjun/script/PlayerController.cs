using System;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //싱글톤 선언
    public static PlayerController instance { get; private set; }

    public int hp;
    private bool isDead;

    public float moveSpeed = 4f;
    public float jumpForce = 4f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private BoxCollider2D boxCollider;

    //총 요소
    public int normalAttackRate;
    public int semiAttackRate;
    public int chargedAttackRate;
    public GameObject bulletPrefab;
    public GameObject chargedBulletPrefab;
    public GameObject semiChargedBP;
    public Transform firePoint;
    public float bulletSpeed;
    public float chargedBulletSpeed;
    public float maxChargeTime;

    public Animator gunAnimator;
    public Animator gunVFXAnimator;
    private float chargedTime;

    //색적 요소
    public float detectionRadius = 5f; // 감지 반경
    private Transform closestEnemy;
    private bool flip;

    public float knockBack;
    private int accel;
    private bool isHitting;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        isDead = false;

        flip = true;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        isHitting = false;
        accel = 1;
    }

    void Update()
    {
        if (isDead) return;
        //좌우 이동
        Move();

        // 점프
        Jump();

        // 총 발사
        Shoot();

        //시야 위/아래로 전환
        CameraControl();

        FindClosestEnemy();
        if (closestEnemy != null)
        {
            flip = false;
            LookAtEnemy();
            FlipTowardsEnemy();
        }
        else
        {
            transform.Find("Gun").transform.rotation = Quaternion.identity;
            transform.Find("Gun").transform.localScale = Vector3.one;
            flip = true;
        }

    }

    void FixedUpdate()
    {
        // 땅 체크
        isGrounded = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < detectionRadius && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
    }

    void LookAtEnemy()
    {
        Transform gunTransform = transform.Find("Gun").transform;
        Vector3 direction = closestEnemy.position - gunTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void FlipTowardsEnemy()
    {
        Vector3 enemyPosition = closestEnemy.position;
        Vector3 scale = transform.localScale;
        Vector3 gunScale = transform.Find("Gun").transform.localScale;

        if (enemyPosition.x < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
            transform.Find("Gun").transform.position = transform.position + new Vector3(0.1f, -0.275f, 0);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
            transform.Find("Gun").transform.position = transform.position + new Vector3(-0.1f, -0.275f, 0);
        }
        firePoint.rotation = new Quaternion(0, 0, 0, 0);

        transform.localScale = scale;
        transform.Find("Gun").transform.localScale = scale / 3;
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isWalking", true);
            if (flip) transform.localScale = new Vector3(-3, 3, 0);
            if (flip) firePoint.rotation = new Quaternion(0, 0, 180, 0);
            transform.Translate(Vector3.right * -1 * moveSpeed * Time.deltaTime);

            // 총 위치 조정
            if (flip) transform.Find("Gun").transform.position = transform.position + new Vector3(0.1f, -0.275f, 0);

            //피격시 넉백 방향 조정
            accel = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isWalking", true);
            if (flip) transform.localScale = new Vector3(3, 3, 0);
            if (flip) firePoint.rotation = new Quaternion(0, 0, 0, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            //총 위치 조정
            if (flip) transform.Find("Gun").transform.position = transform.position + new Vector3(-0.1f, -0.275f, 0);

            //피격시 넉백 방향 조정
            accel = 1;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
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
            rb.velocity = firePoint.right * chargedBulletSpeed ;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isDead)
        {
            Hit(5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" && !isDead)
        {
            Hit(10);
        }
    }

    private void Hit(int damage)
    {
        isHitting = true;
        transform.Find("Gun").gameObject.SetActive(false);
        animator.SetTrigger("hit");
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - knockBack * accel, transform.position.y, 0), 1f);
        hp -= damage;
        if (hp <= 0f)
        {
            Death();
        }
    }

    public void Death()
    {
        transform.Find("Gun").gameObject.SetActive(false);
        isDead = true;
        animator.SetBool("isDead", true);
        //Destroy(GetComponent<Rigidbody2D>());
    }

    public void back()
    {
        isHitting = false;
        transform.Find("Gun").gameObject.SetActive(true);
        animator.SetTrigger("back");
    }
}