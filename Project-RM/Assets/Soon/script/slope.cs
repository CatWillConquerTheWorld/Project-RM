using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slope : MonoBehaviour
{
    public DistanceJoint2D ropeJoint; // 로프와 연결할 조인트
    public Rigidbody2D playerRb; // 캐릭터의 Rigidbody
    public Rigidbody2D ropeRb;
    public LayerMask ropeLayer; // 로프 레이어 설정
    public float slideSpeed = 2f; // 내려가는 속도 설정
    public Vector2 slideDirection = new Vector2(1f, -1f).normalized;
    public GameObject stopRopeLocation;
    private bool isOnRope = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E 키를 누르면 로프 잡기
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, ropeLayer);

            if (hit.collider != null)
            {
                isOnRope = true;
                ropeJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                ropeJoint.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Space 키를 누르면 로프에서 떨어짐
        {
            isOnRope = false;
            ropeJoint.enabled = false;
            ropeRb.velocity = Vector2.zero;
        }

    }

    void FixedUpdate()
    {
        if (isOnRope)
        {
            // 캐릭터가 로프를 잡고 있을 때 경사면을 미끄러지도록 처리
            playerRb.velocity = slideDirection * slideSpeed;
            ropeRb.velocity = slideDirection * slideSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 객체가 특정 GameObject와 일치하는지 확인
        if (collision.gameObject == stopRopeLocation)
        {
            // 로프의 움직임을 멈춤
            playerRb.velocity = Vector2.zero;
            ropeRb.velocity = Vector2.zero;
            ropeRb.angularVelocity = 0f;
        }
    }
}
