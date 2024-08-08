using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //½Ì±ÛÅæ ¼±¾ð
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

    //ÃÑ ¿ä¼Ò
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

    //»öÀû ¿ä¼Ò
    public float detectionRadius = 5f; // °¨Áö ¹Ý°æ
    private Transform closestEnemy;
    private bool flip;

    public GameObject enemyInfoUI;
    private TextMeshProUGUI enemyName;
    private Image healthBar;
    private TextMeshProUGUI health;

    public float knockBack;
    private bool isHitting;

    private bool align;
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

        if (enemyInfoUI != null)
        {
            enemyName = enemyInfoUI.GetComponent<RectTransform>().Find("Name").gameObject.GetComponent<TextMeshProUGUI>();
            healthBar = enemyInfoUI.GetComponent<RectTransform>().Find("HealthBar").gameObject.GetComponent<Image>();
            health = enemyInfoUI.GetComponent<RectTransform>().Find("Health").gameObject.GetComponent<TextMeshProUGUI>();
        }

            align = false;
    }

    void Update()
    {
        if (isDead) return;
        //ÁÂ¿ì ÀÌµ¿
        Move();

        // Á¡ÇÁ
        Jump();

        // ÃÑ ¹ß»ç
        Shoot();

        FindClosestEnemy();
        if (closestEnemy != null)
        {
            align = true;
            flip = false;
            LookAtEnemy();
            FlipTowardsEnemy();
            EnemyInfo();
        }
        else
        {
            if (enemyInfoUI != null) enemyInfoUI.SetActive(false);
            transform.Find("Gun").transform.rotation = Quaternion.identity;
            transform.Find("Gun").transform.localScale = Vector3.one;
            flip = true;
            if (align)
            {
                if (transform.localScale.x > 0)
                {
                    firePoint.rotation = new Quaternion(0, 0, 0, 0);
                } else if (transform.localScale.x < 0)
                {
                    firePoint.rotation = new Quaternion(0, 0, 180, 0);
                }
                align = false;
            }
        }

    }

    void FixedUpdate()
    {
        // ¶¥ Ã¼Å©
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

            // ÃÑ À§Ä¡ Á¶Á¤
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

            //ÃÑ À§Ä¡ Á¶Á¤
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
            gunAnimator.SetTrigger("back");
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
            Hit(5, accel);
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
            Hit(10, accel);
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
}