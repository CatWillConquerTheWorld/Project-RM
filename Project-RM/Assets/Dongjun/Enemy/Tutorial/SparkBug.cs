using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkBug : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)" || collision.gameObject.name == "SemiChargedBullet(Clone)" || collision.gameObject.name == "ChargedBullet(Clone)")
        {
            print("Entered");
            animator.SetBool("isDead", true);
        }
    }

    public void Dead()
    {
        Tutorial.Instance.deadEnemies += 1;
        gameObject.SetActive(false);
        transform.tag = "Untagged";
        Destroy(GetComponent<Rigidbody2D>());
        boxCollider.enabled = false;
    }
}
