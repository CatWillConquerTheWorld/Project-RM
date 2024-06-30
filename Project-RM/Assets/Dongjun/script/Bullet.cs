using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 10f; // 파괴될 최대 거리
    private Transform playerTransform;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // 플레이어의 Transform을 찾습니다. 여기서는 태그를 이용해 찾습니다.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 플레이어와 총알 사이의 거리 계산
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // 일정 거리 이상이면 총알 파괴
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
