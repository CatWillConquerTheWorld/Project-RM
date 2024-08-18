using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slope : MonoBehaviour
{
    public Transform targetPosition;  // 플레이어가 이동할 목표 위치
    public GameObject player;
    public float speed = 5f;          // 이동 속도
    public bool isOnRope = false;    // 로프를 잡았는지 여부
    public bool isMoving = false;    // 이동 중인지 여부
    private Vector2 moveDirection;    // 이동 방향
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 로프 잡기 (E 키 입력)
        if (isOnRope && Input.GetKeyDown(KeyCode.E))
        {
            isMoving = true;
            moveDirection = (targetPosition.position - transform.position).normalized;
            player.transform.SetParent(transform);

            // 플레이어의 Rigidbody2D를 kinematic으로 전환 (물리적 자유를 제한)
            player.GetComponent<Rigidbody2D>().isKinematic = true;

        }

        // 목표 위치에 도달하면 멈춤
        if (isMoving && Vector2.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;  // 속도를 0으로 설정
        }

        // 로프에서 내려오기 (스페이스바 입력)
        if (isOnRope && Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = false;
            isOnRope = false;
            player.transform.SetParent(null);
            player.GetComponent<Rigidbody2D>().isKinematic = false;

        }

        // 이동 중일 때 이동 처리
        if (isMoving)
        {
            // 중력을 0으로 설정
            rb.velocity = moveDirection * speed;
        }
    }

    // 로프와 충돌 시 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnRope = true;

        }
    }

    // 로프에서 벗어났을 때 처리
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnRope = false;
        }
    }
}
