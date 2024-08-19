using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //싱글톤 선언
    //public static PlayerController instance { get; private set; }

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
    public static int normalAttackRate;
    public static int semiAttackRate;
    public static int chargedAttackRate;
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
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    private bool flip;

    public GameObject enemyInfoUI;
    private TextMeshProUGUI enemyName;
    private Image healthBar;
    private TextMeshProUGUI health;

    public float knockBack;
    private bool isHitting;

    private bool align;

    private float longNotePrepareMultiplier;
    private float longNoteEndMultiplier;
    private bool isLongAttackCanceled;
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
        isLongAttackCanceled = false;
        isDead = false;

        flip = true;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        isHitting = false;

        if (enemyInfoUI != null)
        {
            enemyName = enemyInfoUI.GetComponent<RectTransform>().Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
            healthBar = enemyInfoUI.GetComponent<RectTransform>().Find("HealthBar").gameObject.GetComponent<Image>();
            health = enemyInfoUI.GetComponent<RectTransform>().Find("Health").gameObject.GetComponent<TextMeshProUGUI>();
        }

        normalAttackRate = 10;
        semiAttackRate = 20;
        chargedAttackRate = 30;

        align = false;
    }

    void Update()
    {
        if (isDead) return;
        //좌우 이동
        Move();

        // 점프
        Jump();

        // 총 발사
        //Shoot();

        //FindClosestEnemy();
        //if (closestEnemy != null)
        //{
        //    align = true;
        //    flip = false;
        //    LookAtEnemy();
        //    FlipTowardsEnemy();
        //    EnemyInfo();
        //}
        //else
        //{
        //    if (enemyInfoUI != null) enemyInfoUI.SetActive(false);
        //    transform.Find("Gun").transform.rotation = Quaternion.identity;
        //    transform.Find("Gun").transform.localScale = Vector3.one;
        //    flip = true;
        //    if (align)
        //    {
        //        if (transform.localScale.x > 0)
        //        {
        //            firePoint.rotation = new Quaternion(0, 0, 0, 0);
        //        } else if (transform.localScale.x < 0)
        //        {
        //            firePoint.rotation = new Quaternion(0, 0, 180, 0);
        //        }
        //        align = false;
        //    }
        //}

    }

    void FixedUpdate()
    {
        // 땅 체크
        isGrounded = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
    }

    void FindClosestEnemy()
    {
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //float closestDistance = Mathf.Infinity;
        //closestEnemy = null;

        //foreach (GameObject enemy in enemies)
        //{
        //    float distance = Vector2.Distance(transform.position, enemy.transform.position);
        //    if (distance < detectionRadius && distance < closestDistance)
        //    {
        //        closestDistance = distance;
        //        closestEnemy = enemy.transform;
        //    }
        //}

        //Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        //closestEnemy = null;
        //foreach (Collider2D enemy in enemies)
        //{
        //    Vector2 directionToEnemy = enemy.transform.position - transform.position;
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToEnemy.normalized, detectionRadius, groundLayer);

        //    // 적과 플레이어 사이에 장애물이 없으면 적을 감지
        //    if (hit.collider == null)
        //    {
        //        closestEnemy = enemy.transform;
        //    }
        //}
        closestEnemy = null;
        for (int i = 0; i < 36; i++)
        {
            // 각 레이의 각도 계산
            float angle = i * (360f / 36);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // 레이캐스트 쏘기
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, groundLayer);

            // 레이캐스트가 장애물에 닿지 않은 경우
            if (hit.collider == null)
            {
                // 레이캐스트 끝 점
                Vector2 endPoint = (Vector2)transform.position + direction * detectionRadius;
                // 적 탐지
                Collider2D[] enemies = Physics2D.OverlapCircleAll(endPoint, 0.1f, enemyLayer);

                foreach (Collider2D enemy in enemies)
                {
                    closestEnemy = enemy.transform;
                }
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

     void EnemyInfo()
    {
        if (enemyInfoUI == null) return;
        float enemyHp = closestEnemy.GetComponent<EnemyController>().hp;
        float enemyFullHp = closestEnemy.GetComponent<EnemyController>().fullHp;
        string _enemyName = closestEnemy.name.Substring(0,closestEnemy.name.IndexOf("("));
        enemyName.text = _enemyName;
        health.text = enemyHp + " / " + enemyFullHp;
        healthBar.fillAmount = enemyHp / enemyFullHp;

        print("this is " + enemyHp + " and " + enemyFullHp + " also " + enemyHp / enemyFullHp);
        enemyInfoUI.SetActive(true);
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("isWalking", false);
                return;
            }
            animator.SetBool("isWalking", true);
            if (flip) transform.localScale = new Vector3(-3, 3, 0);
            if (flip) firePoint.rotation = new Quaternion(0, 0, 180, 0);
            transform.Translate(Vector3.right * -1 * moveSpeed * Time.deltaTime);

            // 총 위치 조정
            if (flip) transform.Find("Gun").transform.position = transform.position + new Vector3(0.1f, -0.275f, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                animator.SetBool("isWalking", false);
                return;
            }
            animator.SetBool("isWalking", true);
            if (flip) transform.localScale = new Vector3(3, 3, 0);
            if (flip) firePoint.rotation = new Quaternion(0, 0, 0, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            //총 위치 조정
            if (flip) transform.Find("Gun").transform.position = transform.position + new Vector3(-0.1f, -0.275f, 0);
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

    //private void Shoot()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        chargedTime = 0;
    //        gunAnimator.SetBool("isCharging", true);
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        chargedTime += Time.deltaTime;
    //    }
    //    if (Input.GetKeyUp(KeyCode.A))
    //     {
    //        if (chargedTime >= maxChargeTime)
    //        {
    //            GunEffect("Charged");
    //            gunAnimator.SetBool("isCharging", false);
    //            Attack("Charged");
    //        } 
    //        else if (chargedTime >= maxChargeTime / 3 * 2)
    //        {
    //            GunEffect("Charged");
    //            gunAnimator.SetBool("isCharging", false);
    //            Attack("SemiCharged");
    //        }
    //        else
    //        {
    //            GunEffect("Normal");
    //            gunAnimator.SetBool("isCharging", false);
    //            Attack("Normal");
    //        }
    //    }
    //}

    public void ShortAttack(float multiplier)
    {
        GunEffect("Normal");
        gunAnimator.SetBool("isCharging", false);
        Attack("Normal", multiplier);
        gunAnimator.SetTrigger("back");
    }

    public void LongAttackPrepare(float multiplier)
    {
        isLongAttackCanceled = false;
        longNotePrepareMultiplier = multiplier;
        gunAnimator.SetBool("isCharging", true);
    }

    public void LongAttack(float multiplier)
    {
        if (isLongAttackCanceled)
        {
            isLongAttackCanceled = false;
            gunAnimator.SetBool("isCharging", false);
            gunAnimator.SetTrigger("back");
            return;
        }
        longNoteEndMultiplier = multiplier;
        float finalMultiplier = longNoteEndMultiplier * longNotePrepareMultiplier;
        gunAnimator.SetBool("isCharging", false);
        gunAnimator.SetTrigger("back");
        GunEffect("Charged");
        if (finalMultiplier >= 1) Attack("Charged", finalMultiplier);
        else Attack("SemiCharged", finalMultiplier);
    }

    public void LongAttackCancel()
    {
        isLongAttackCanceled = true;
        gunAnimator.SetBool("isCharging", false);
        gunAnimator.SetTrigger("back");
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

    private void Attack(string type, float multiplier)
    {
        print("Done" + type);
        if (type == "Charged")
        {
            GameObject chargedBullet = Instantiate(chargedBulletPrefab, firePoint.position, firePoint.rotation);
            chargedBullet.SetActive(true);
            chargedBullet.GetComponent<Bullet>().myAttackRate = 30 * multiplier;
            Rigidbody2D rb = chargedBullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * chargedBulletSpeed ;
            chargedBullet.GetComponent<Animator>().SetTrigger("charged");
        } 
        else if (type == "SemiCharged")
        {
            GameObject semiChargedBullet = Instantiate(semiChargedBP, firePoint.position, firePoint.rotation);
            semiChargedBullet.SetActive(true);
            semiChargedBullet.GetComponent<Bullet>().myAttackRate = 20 * multiplier;
            Rigidbody2D rb = semiChargedBullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * chargedBulletSpeed;
            semiChargedBullet.GetComponent<Animator>().SetTrigger("semi");
        }
        else if (type == "Normal")
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().myAttackRate = 10 * multiplier;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * bulletSpeed;
            bullet.GetComponent<Animator>().SetTrigger("normal");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int accel = 0;
        if (collision.gameObject.tag == "Enemy" && !isDead)
        {
            if (transform.position.x > collision.transform.position.x)
            {
                accel = -1;
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                accel = 1;
            }
            Hit(2, accel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int accel = 0;
        if (collision.gameObject.tag == "EnemyBullet" && !isDead)
        {
            if (transform.position.x > collision.transform.position.x)
            {
                accel = -1;
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                accel = 1;
            }
            Hit(4, accel);
        }
    }

    private void Hit(int damage, int accelerator)
    {
        isHitting = true;
        transform.Find("Gun").gameObject.SetActive(false);
        animator.SetTrigger("hit");
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - knockBack * accelerator, transform.position.y, 0), 1f);
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

    public bool GetIsDead()
    {
        return isDead;
    }
}