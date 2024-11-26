using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    BoxCollider2D col;
    //public GameObject laserPrefab; // 레이저 프리팹
    //public GameObject dangerPrefab; // Danger 이미지 프리팹
    //public Transform laserSpawnPoint; // 레이저 발사 위치
    public float laserDelay = 1.0f; // Danger 이미지가 사라진 후 레이저가 발사되는 딜레이
    public float dangerDuration = 1.5f; // Danger 이미지 표시 시간
    public float laserDuration = 2f;
    public int laserCount = 3; // 레이저 발사 횟수

    public float maxHealth = 500f;  // 보스의 최대 체력
    public float currentHealth;

    public GameObject spitCollider;
    public GameObject stumpCollider;
    public GameObject buffCollider;
    public GameObject jumpCollider;
    public Image healthBarFiller;
    public Transform player;

    private void Awake()
    {
        spitCollider.SetActive(false);
        stumpCollider.SetActive(false);
        buffCollider.SetActive(false);
        jumpCollider.SetActive(false);
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBarFiller.fillAmount = currentHealth / maxHealth;
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-4, 4, 1); // X축 반전
        }
        else // 플레이어가 보스의 오른쪽에 있으면 보스가 오른쪽을 보게 함
        {
            transform.localScale = new Vector3(4, 4, 1); // 기본 방향
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Attack2();
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Attack1();
        //}
        
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Jump();
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    EndJump();
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Buff();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    currentHealth -= 50;
        //}

        //if (HP <= 0)
        //{
        //    Die();
        //}

    }

    public void Jump()
    {
        animator.SetBool("IsJump", true);
        print("Fuck~");
    }

    public void Attack1()
    {
        spitCollider.SetActive(true);
        animator.SetTrigger("Attack1");
    }

    public void Attack1Back()
    {
        spitCollider.SetActive(false);
    }

    public void Attack2()
    {
        stumpCollider.SetActive(true);
        animator.SetTrigger("Attack2");
    }

    public void Attack2Back()
    {
        stumpCollider.SetActive(false);
    }

    public void Buff()
    {
        buffCollider.SetActive(true);
        animator.SetTrigger("buff");
    }

    public void BuffBack()
    {
        buffCollider.SetActive(false);
    }

    public void EndJump()
    {
        if (transform.localScale.x < 0) //왼쪽
        {
            Vector3 newPosition = new Vector3(player.position.x + 2f, player.position.y + 1f, player.position.z);
            transform.position = newPosition;
        }
        else
        {
            Vector3 newPosition = new Vector3(player.position.x + 2f, player.position.y + 1f, player.position.z);
            transform.position = newPosition;
        }
        
        jumpCollider.SetActive(true);
        animator.SetTrigger("Land");
        animator.SetBool("IsJump", false);
        print("Shit!!!!!!!!!!");
    }

    public void EndJumpEnd()
    {
        jumpCollider.SetActive(false);
    }

    public void Back()
    {
        animator.SetTrigger("Back");
    }

    public void Die()
    {
        animator.SetTrigger("die");
        col.enabled = false;
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

    public void SpitColliderActivate()
    {

    }
}
