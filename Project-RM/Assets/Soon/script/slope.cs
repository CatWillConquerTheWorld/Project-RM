using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slope : MonoBehaviour
{
    public DistanceJoint2D ropeJoint; // ������ ������ ����Ʈ
    public Rigidbody2D playerRb; // ĳ������ Rigidbody
    public Rigidbody2D ropeRb;
    public LayerMask ropeLayer; // ���� ���̾� ����
    public float slideSpeed = 2f; // �������� �ӵ� ����
    public Vector2 slideDirection = new Vector2(1f, -1f).normalized;
    public GameObject stopRopeLocation;
    private bool isOnRope = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E Ű�� ������ ���� ���
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, ropeLayer);

            if (hit.collider != null)
            {
                isOnRope = true;
                ropeJoint.connectedBody = hit.collider.GetComponent<Rigidbody2D>();
                ropeJoint.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Space Ű�� ������ �������� ������
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
            // ĳ���Ͱ� ������ ��� ���� �� ������ �̲��������� ó��
            playerRb.velocity = slideDirection * slideSpeed;
            ropeRb.velocity = slideDirection * slideSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ��ü�� Ư�� GameObject�� ��ġ�ϴ��� Ȯ��
        if (collision.gameObject == stopRopeLocation)
        {
            // ������ �������� ����
            playerRb.velocity = Vector2.zero;
            ropeRb.velocity = Vector2.zero;
            ropeRb.angularVelocity = 0f;
        }
    }
}
