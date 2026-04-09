using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 500f;  // 보스의 최대 체력
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            //Attack2();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //Attack1();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            //Jump();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            //EndJump();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            //Buff();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            currentHealth -= 50;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //animator.SetTrigger("die");
        //col.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            currentHealth -= collision.gameObject.GetComponent<Bullet>().myAttackRate;
            //animator.SetTrigger("hit");
        }
        else if (collision.gameObject.name == "SemiChargedBullet(Clone)")
        {
            currentHealth -= collision.gameObject.GetComponent<Bullet>().myAttackRate;
            //animator.SetTrigger("hit");
        }
        else if (collision.gameObject.name == "ChargedBullet(Clone)")
        {
            currentHealth -= collision.gameObject.GetComponent<Bullet>().myAttackRate;
            //animator.SetTrigger("hit");
        }
    }
}
