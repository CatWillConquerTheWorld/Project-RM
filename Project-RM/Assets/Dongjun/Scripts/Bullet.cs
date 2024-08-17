using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 10f; // �ı��� �ִ� �Ÿ�
    private Transform playerTransform;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // �÷��̾��� Transform�� ã���ϴ�. ���⼭�� �±׸� �̿��� ã���ϴ�.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // �÷��̾�� �Ѿ� ������ �Ÿ� ���
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // ���� �Ÿ� �̻��̸� �Ѿ� �ı�
        if (distance > maxDistance)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(this.GetComponent<BoxCollider2D>());
            animator.SetTrigger("Destroy");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
