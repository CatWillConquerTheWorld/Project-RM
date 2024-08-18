using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : MonoBehaviour
{
    public int HP;
    public Animator animator;
    public Rigidbody2D rigid;
    public Ghost ghost;
    public float moveSpeed;
    public float maxDashTime;
    public bool makeGhost = false;
    public bool isDash = false;
    private float dashTime = 0;
    public Vector2 tmpDir;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetKeyDown(KeyCode.A))
        {
            Dash();
            Attack1();
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Special();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            HP -= 50;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Hit();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }

        if (HP <= 0)
        {
            Die();
        }
    }
    void Attack1()
    {
        animator.SetTrigger("Attack1");
    }

    void Special()
    {
        animator.SetTrigger("SpecialAttack");
    }

    void Dash()
    {
        this.ghost.makeGhost = true;
        this.dashTime += Time.deltaTime;
        this.isDash = true;

        if (this.tmpDir == Vector2.zero || this.tmpDir == new Vector2(1f, 0f))
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

    void Die()
    {
        animator.SetTrigger("Death");
    }

    void Hit()
    {
        animator.SetTrigger("Hit");
    }
}
