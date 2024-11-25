using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    //public GameObject laserPrefab; // 레이저 프리팹
    //public GameObject dangerPrefab; // Danger 이미지 프리팹
    //public Transform laserSpawnPoint; // 레이저 발사 위치
    public float laserDelay = 1.0f; // Danger 이미지가 사라진 후 레이저가 발사되는 딜레이
    public float dangerDuration = 1.5f; // Danger 이미지 표시 시간
    public float laserDuration = 2f;
    public int laserCount = 3; // 레이저 발사 횟수

    public float maxHealth = 500f;  // 보스의 최대 체력
    public float currentHealth;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack2();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Attack1();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Buff();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentHealth -= 50;
        }

        //if (HP <= 0)
        //{
        //    Die();
        //}

    }

    public void Jump()
    {
        animator.SetBool("IsJump", true);
    }

    public void Attack1()
    {
        animator.SetTrigger("Attack1");
    }

    public void Attack2()
    {
        animator.SetTrigger("Attack2");
    }

    public void Buff()
    {
        animator.SetTrigger("buff");
    }

    public void EndJump()
    {
        animator.SetBool("IsJump", false);
    }

    public void Back()
    {
        animator.SetTrigger("Back");
    }

    public void Die()
    {
        animator.SetTrigger("die");
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
}
