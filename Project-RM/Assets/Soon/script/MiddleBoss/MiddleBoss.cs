using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleBoss : MonoBehaviour
{
    public Animator animator;
    Animator SRanimator;
    Animator SLanimator;
    public Rigidbody2D rigid;
    public Ghost ghost;
    public float moveSpeed;
    public float maxDashTime;
    public float waitTP = 0.5f;
    public bool makeGhost = false;
    public bool isDash = false;
    private float dashTime = 0;
    public Vector2 tmpDir;

    private PolygonCollider2D polygonCollider;
    private GameObject attack1Collider;
    private GameObject attack2Collider;
    private GameObject specialCollider;
    public GameObject special2RCollider;
    public GameObject special2LCollider;
    public GameObject[] DangerSqaures;
    public GameObject portal;

    public Image healthBarFiller;

    public Transform player;  // 플레이어의 위치를 참조하기 위한 변수
    public float offsetX = 2f;

    public float maxHealth = 500f;  // 보스의 최대 체력
    public float currentHealth;

    public int attackStack;

    public GameObject clock;
    public GameObject firstPin;
    public GameObject secondPin;

    public GameObject underTilemap;
    public GameObject underTilemapDec;
    public GameObject leftTilemap;
    public GameObject leftTilemapDec;
    public GameObject rightTilemap;
    public GameObject rightTilemapDec;

    public GameObject enemiesUnder;
    public GameObject enemiesLeft;
    public GameObject enemiesRight;

    // Start is called before the first frame update
    void Awake()
    {
        special2RCollider = transform.Find("specialCollider2_R").gameObject;
        special2LCollider = transform.Find("specialCollider2_L").gameObject;
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        SRanimator = special2RCollider.GetComponent<Animator>();
        SLanimator = special2LCollider.GetComponent<Animator>();
        attack1Collider = transform.Find("attack1Collider").gameObject;
        attack2Collider = transform.Find("attack2Collider").gameObject;
        specialCollider = transform.Find("specialCollider").gameObject;
        currentHealth = maxHealth;
    }

    void Start()
    {

        attackStack = 0;
    }

    // Update is called once per frame
    void Update()
    {

        healthBarFiller.fillAmount = currentHealth / maxHealth;

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    //Dash();
        //    Attack1();
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Attack2();
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Special();
        //}

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    currentHealth -= 50;
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Hit();
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{

        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    StartCoroutine("Teleport");
        //}

        //if (HP <= 0)
        //{
        //    Die();
        //}
    }
    public void Attack1()
    {
        //DangerSqaures[0].SetActive(true);
        //yield return new WaitForSeconds(0.3f);
        //DangerSqaures[0].SetActive(false);
        //yield return null;
        this.rigid.velocity = Vector2.zero; // 대쉬 종료 후 속도 0
        attack1Collider.SetActive(true);
        animator.SetTrigger("Attack1");
    }

    public void Attack2()
    {
        //DangerSqaures[1].SetActive(true);
        //yield return new WaitForSeconds(0.3f);
        //DangerSqaures[1].SetActive(false);
        //yield return null;
        this.rigid.velocity = Vector2.zero; // 대쉬 종료 후 속도 0
        attack2Collider.SetActive(true);
        animator.SetTrigger("Attack2");
    }

    public void Special()
    {
        //DangerSqaures[2].SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //DangerSqaures[2].SetActive(false);
        this.rigid.velocity = Vector2.zero; // 대쉬 종료 후 속도 0
        specialCollider.SetActive(true);
        animator.SetTrigger("SpecialAttack");
    }

    IEnumerator Special2R()
    {
        //DangerSqaures[3].SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //DangerSqaures[3].SetActive(false);
        //yield return new WaitForSeconds(0.1f);
        special2RCollider.SetActive(false);
        special2RCollider.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        special2RCollider.SetActive(false);
    }

    IEnumerator Special2L()
    {
        //DangerSqaures[4].SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //DangerSqaures[4].SetActive(false);
        //yield return new WaitForSeconds(0.1f);
        special2LCollider.SetActive(false);
        special2LCollider.SetActive(true);
        SLanimator.SetTrigger("Special2L");
        yield return new WaitForSeconds(0.75f);
        special2LCollider.SetActive(false);
    }

    IEnumerator Teleport()
    {
        polygonCollider.enabled = false;
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.4f);

        int direction = Random.value > 0.5f ? 1 : -1;
        Vector3 newPosition = new Vector3(player.position.x + direction * offsetX, transform.position.y, transform.position.z);
        transform.position = newPosition;

        yield return new WaitForSeconds(waitTP);
        DangerSqaures[5].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DangerSqaures[5].SetActive(false);
        animator.SetTrigger("Appear");
        polygonCollider.enabled = true;
    }

    public void Dash()
    {
        this.rigid.velocity = Vector2.zero; // 대쉬 종료 후 속도 0
        this.ghost.makeGhost = true;
        this.dashTime += Time.deltaTime;
        this.isDash = true;

        if (player.transform.position.x < transform.position.x)
        {
            this.tmpDir = Vector2.left;
        }
        else
        {
            this.tmpDir = Vector2.right;

        }

        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        //yield return new WaitForSeconds(0.4f);
        while (this.dashTime < this.maxDashTime)
        {
            this.dashTime += Time.deltaTime;
            this.rigid.velocity = this.tmpDir.normalized * (this.moveSpeed * 7);
            yield return null; // 다음 프레임까지 기다림
        }

        this.rigid.velocity = Vector2.zero; // 대쉬 종료 후 속도 0
        this.isDash = false;
        this.ghost.makeGhost = false;
        this.dashTime = 0;


    }

    public void Die()
    {
        animator.SetTrigger("Death");
        portal.SetActive(true);
        attack1Collider.SetActive(false);
        attack2Collider.SetActive(false);
        specialCollider.SetActive(false);
        GetComponent<PolygonCollider2D>().enabled = false;
        StartCoroutine(DemoEnd());
    }

    IEnumerator DemoEnd()
    {
        yield return new WaitForSeconds(1f);
        SceneLoader.LoadSceneWithLoading("DemoEnd");
    }

    void Hit()
    {
        animator.SetTrigger("Hit");
    }

    public void Attack1False()
    {
        animator.SetTrigger("back");
        attack1Collider.SetActive(false);
    }

    public void Attack2False()
    {
        animator.SetTrigger("back");
        attack2Collider.SetActive(false);
    }

    public void SpecialFalse()
    {
        animator.SetTrigger("SpecialBack");
        specialCollider.SetActive(false);
    }

    public void back()
    {
        animator.SetTrigger("back");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            currentHealth -= collision.gameObject.GetComponent<Bullet>().myAttackRate;
            animator.SetTrigger("hit");
        }
        else if (collision.gameObject.name == "SemiChargedBullet(Clone)")
        {
            currentHealth -= collision.gameObject.GetComponent<Bullet>().myAttackRate;
            animator.SetTrigger("hit");
        }
        else if (collision.gameObject.name == "ChargedBullet(Clone)")
        {
            currentHealth -= collision.gameObject.GetComponent<Bullet>().myAttackRate;
            animator.SetTrigger("hit");
        }
    }

    public void ClockPattern()
    {
        firstPin.transform.Rotate(0, 0, 360f / 62f);
        secondPin.transform.Rotate(0, 0, -360f /62f);
    }

    public void TilemapsInitiate()
    {
        for (int i = 0; i < enemiesUnder.transform.childCount; i++)
        {
            enemiesUnder.transform.GetChild(i).transform.DOMoveY(0.3f, 2f).SetEase(Ease.Linear);
        }
        for (int i = 0; i < enemiesLeft.transform.childCount; i++)
        {
            enemiesLeft.transform.GetChild(i).transform.DOMoveX(enemiesLeft.transform.GetChild(i).transform.position.x + 7f, 2f).SetEase(Ease.Linear);
        }
        for (int i = 0; i < enemiesRight.transform.childCount; i++)
        {
            enemiesRight.transform.GetChild(i).transform.DOMoveX(enemiesRight.transform.GetChild(i).transform.position.x - 7f, 2f).SetEase(Ease.Linear);
        }
        Camera.main.DOShakePosition(2f, 0.1f, 20, 90, false);
        underTilemap.transform.DOMoveY(0, 2f).SetEase(Ease.Linear);
        underTilemapDec.transform.DOMoveY(0, 2f).SetEase(Ease.Linear);
        leftTilemap.transform.DOMoveX(0, 2f).SetEase(Ease.Linear);
        leftTilemapDec.transform.DOMoveX(0, 2f).SetEase(Ease.Linear);
        rightTilemap.transform.DOMoveX(0, 2f).SetEase(Ease.Linear);
        rightTilemapDec.transform.DOMoveX(0, 2f).SetEase(Ease.Linear).OnComplete(() => EnableEnemies());

    }

    void EnableEnemies()
    {
        print("Done!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        for (int i = 0; i < enemiesUnder.transform.childCount; i++)
        {
            enemiesUnder.transform.GetChild(i).GetComponent<Animator>().SetBool("initiated", true);
            enemiesUnder.transform.GetChild(i).GetComponent<EnemyController>().enabled = true;
            enemiesUnder.transform.GetChild(i).GetComponent<Cage>().enabled = true;
            //enemiesUnder.transform.GetChild(i).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            enemiesUnder.transform.GetChild(i).tag = "Enemy";
            enemiesUnder.transform.GetChild(i).gameObject.layer = 7;
            enemiesUnder.transform.GetChild(i).transform.Find("attackCollider").tag = "EnemyBullet";
        }

        for (int i = 0; i < enemiesLeft.transform.childCount; i++)
        {
            enemiesLeft.transform.GetChild(i).GetComponent<Animator>().SetBool("initiated", true);
            enemiesLeft.transform.GetChild(i).GetComponent<EnemyController>().enabled = true;
            enemiesLeft.transform.GetChild(i).GetComponent<Cage>().enabled = true;
            //enemiesLeft.transform.GetChild(i).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            enemiesLeft.transform.GetChild(i).tag = "Enemy";
            enemiesLeft.transform.GetChild(i).gameObject.layer = 7;
            enemiesLeft.transform.GetChild(i).transform.Find("attackCollider").tag = "EnemyBullet";
        }

        for (int i = 0; i < enemiesRight.transform.childCount; i++)
        {
            enemiesRight.transform.GetChild(i).GetComponent<Animator>().SetBool("initiated", true);
            enemiesRight.transform.GetChild(i).GetComponent<EnemyController>().enabled = true;
            enemiesRight.transform.GetChild(i).GetComponent<Cage>().enabled = true;
            //enemiesRight.transform.GetChild(i).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            enemiesRight.transform.GetChild(i).tag = "Enemy";
            enemiesRight.transform.GetChild(i).gameObject.layer = 7;
            enemiesRight.transform.GetChild(i).transform.Find("attackCollider").tag = "EnemyBullet";
        }
    }

    public void TilemapsDisinitiate()
    {
        CheckEnemies();
        Camera.main.DOShakePosition(1f, 0.1f, 40, 90, false);
        underTilemap.transform.DOMoveY(-4, 1f).SetEase(Ease.Linear);
        underTilemapDec.transform.DOMoveY(-4, 1f).SetEase(Ease.Linear);
        leftTilemap.transform.DOMoveX(7, 1f).SetEase(Ease.Linear);
        leftTilemapDec.transform.DOMoveX(7, 1f).SetEase(Ease.Linear);
        rightTilemap.transform.DOMoveX(-7, 1f).SetEase(Ease.Linear);
        rightTilemapDec.transform.DOMoveX(-7, 1f).SetEase(Ease.Linear);
    }

    private void CheckEnemies()
    {
        float amount = 0f;
        for (int i = 0; i < enemiesUnder.transform.childCount; i++)
        {
            if (!enemiesUnder.transform.GetChild(i).GetComponent<EnemyController>().isDead) amount += 5f;
            enemiesUnder.transform.GetChild(i).GetComponent<Animator>().SetTrigger("hit");
            enemiesUnder.transform.GetChild(i).GetComponent<EnemyController>().Death();
        }
        for (int i = 0; i < enemiesLeft.transform.childCount; i++)
        {
            if (!enemiesLeft.transform.GetChild(i).GetComponent<EnemyController>().isDead) amount += 5f;
            enemiesLeft.transform.GetChild(i).GetComponent<Animator>().SetTrigger("hit");
            enemiesLeft.transform.GetChild(i).GetComponent<EnemyController>().Death();
        }
        for (int i = 0; i < enemiesRight.transform.childCount; i++)
        {
            if (!enemiesRight.transform.GetChild(i).GetComponent<EnemyController>().isDead) amount += 5f;
            enemiesRight.transform.GetChild(i).GetComponent<Animator>().SetTrigger("hit");
            enemiesRight.transform.GetChild(i).GetComponent<EnemyController>().Death();
        }
        player.GetComponent<PlayerController>().DealHP(amount);
    }
}
