using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slope : MonoBehaviour
{
    public DistanceJoint2D ropeJoint; // ������ ������ ����Ʈ
    public Rigidbody2D rb; // ĳ������ Rigidbody
    public LayerMask ropeLayer; // ���� ���̾� ����
    public float slideSpeed = 2f; // �������� �ӵ� ����
    public Vector2 slideDirection = new Vector2(1f, -1f).normalized;
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
        }
    }

    void FixedUpdate()
    {
        if (isOnRope)
        {
            // ĳ���Ͱ� ������ ��� ���� �� ������ �̲��������� ó��
            rb.velocity = slideDirection * slideSpeed;
        }
    }
}
