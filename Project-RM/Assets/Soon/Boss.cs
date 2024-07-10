using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Attack1();
        }

    }


    void Jump()
    {
        animator.SetBool("IsJump", true);
    }

    void Attack1()
    {
        animator.SetTrigger("Attack2");
    }

    void EndJump()
    {
        animator.SetBool("IsJump", false);
    }

    void Back()
    {
        animator.SetTrigger("Back");
    }
}
