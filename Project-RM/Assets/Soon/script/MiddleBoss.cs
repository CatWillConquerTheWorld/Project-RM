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

    public float maxHealth = 100f;  // 보스의 최대 체력
    public float currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        SRanimator = special2RCollider.GetComponent<Animator>();
        SLanimator = special2LCollider.GetComponent<Animator>();
        attack1Collider = transform.Find("attack1Collider").gameObject;
        attack2Collider = transform.Find("attack2Collider").gameObject;
        specialCollider = transform.Find("specialCollider").gameObject;
        special2RCollider = transform.Find("specialCollider2_R").gameObject;
        special2LCollider = transform.Find("specialCollider2_L").gameObject;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        healthBarFiller.fillAmount = currentHealth / maxHealth;

        if (Input.GetKeyDown(KeyCode.A))
        {
            Dash();
            StartCoroutine(Attack1());          
        }

        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            StartCoroutine(Attack2()); 
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Special());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentHealth -= 50;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Hit();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("Teleport");
        }

        //if (HP <= 0)
        //{
        //    Die();
        //}
    }
    IEnumerator Attack1()
    {
        DangerSqaures[0].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        DangerSqaures[0].SetActive(false);
        yield return null;
        attack1Collider.SetActive(true);
        animator.SetTrigger("Attack1");
    }

    IEnumerator Attack2()
    {
        DangerSqaures[1].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        DangerSqaures[1].SetActive(false);
        yield return null;
        attack2Collider.SetActive(true);
        animator.SetTrigger("Attack2");
    }

    IEnumerator Special()
    {
        DangerSqaures[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DangerSqaures[2].SetActive(false);
        specialCollider.SetActive(true);
        animator.SetTrigger("SpecialAttack");
    }

    IEnumerator Special2R()
    {
        DangerSqaures[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DangerSqaures[3].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        special2RCollider.SetActive(true);
        SRanimator.SetTrigger("Special2R");
        yield return new WaitForSeconds(1f);
        special2RCollider.SetActive(false);
    }

    IEnumerator Special2L()
    {
        DangerSqaures[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DangerSqaures[4].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        special2LCollider.SetActive(true);
        SLanimator.SetTrigger("Special2L");
        yield return new WaitForSeconds(1f);
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

    void Dash()
    {
        this.ghost.makeGhost = true;
        this.dashTime += Time.deltaTime;
        this.isDash = true;

        if (transform.localScale.x <= 0f)
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
        yield return new WaitForSeconds(0.4f);
        while (this.dashTime < this.maxDashTime)
        {
            this.dashTime += Time.deltaTime;
            this.rigid.velocity = this.tmpDir.normalized * (this.moveSpeed * 5);
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
    }

    void Hit()
    {
        animator.SetTrigger("Hit");
    }

    void Attack1False()
    {
        attack1Collider.SetActive(false);
    }

    void Attack2False()
    {
        attack2Collider.SetActive(false);
    }

    void SpecialFalse()
    {
        specialCollider.SetActive(false);
    }
}
