using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slope : MonoBehaviour
{
    public Transform targetPosition;  // �÷��̾ �̵��� ��ǥ ��ġ
    public GameObject player;
    public float speed = 5f;          // �̵� �ӵ�
    public bool isOnRope = false;    // ������ ��Ҵ��� ����
    public bool isMoving = false;    // �̵� ������ ����
    public bool isRiding = false;
    private Vector2 moveDirection;    // �̵� ����
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���� ��� (E Ű �Է�)
        // F Ű �Է����� ���� Ÿ��/������
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isRiding)
            {
                // �������� ��������
                isMoving = false;
                isOnRope = false;
                isRiding = false;
                player.transform.SetParent(null);  // �θ𿡼� �и�
                player.GetComponent<Rigidbody2D>().isKinematic = false;  // ���� Ȱ��ȭ
            }
            else
            {
                // ���� Ÿ��
                isOnRope = true;
                isMoving = true;
                isRiding = true;
                moveDirection = (targetPosition.position - transform.position).normalized;
                player.transform.SetParent(transform);  // ������ �ڽ����� ����
                player.GetComponent<Rigidbody2D>().isKinematic = true;  // ���� ��Ȱ��ȭ
            }
        }

        // ��ǥ ��ġ�� �����ϸ� ����
        if (isMoving && Vector2.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;  // �ӵ��� 0���� ����
        }

        

        // �̵� ���� �� �̵� ó��
        if (isMoving)
        {
            // �߷��� 0���� ����
            rb.velocity = moveDirection * speed;
        }
    }

    // ������ �浹 �� ó��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnRope = true;

        }
    }

    // �������� ����� �� ó��
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnRope = false;
        }
    }
}
