using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int HP;
    Rigidbody2D rigid;
    Animator animator;
    float maxSpeed = 4.0f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HP = HP - 50;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Attack2();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            Attack1();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Buff();
        }

        if(HP <= 0)
        {
            Die();
        }

    }

    void Jump()
    {
        animator.SetBool("IsJump", true);
    }

    void Attack1()
    {
        animator.SetTrigger("Attack1");
    }

    void Attack2()
    {
        animator.SetTrigger("Attack2");
    }
    
    void Buff()
    {
        animator.SetTrigger("buff");
    }

    void EndJump()
    {
        animator.SetBool("IsJump", false);
    }

    void Back()
    {
        animator.SetTrigger("Back");
    }

    void Die()
    {
        animator.SetTrigger("die");
    }
}
