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
    private Vector2 moveDirection;    // �̵� ����
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���� ��� (E Ű �Է�)
        if (isOnRope && Input.GetKeyDown(KeyCode.E))
        {
            isMoving = true;
            moveDirection = (targetPosition.position - transform.position).normalized;
            player.transform.SetParent(transform);

            // �÷��̾��� Rigidbody2D�� kinematic���� ��ȯ (������ ������ ����)
            player.GetComponent<Rigidbody2D>().isKinematic = true;

        }

        // ��ǥ ��ġ�� �����ϸ� ����
        if (isMoving && Vector2.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;  // �ӵ��� 0���� ����
        }

        // �������� �������� (�����̽��� �Է�)
        if (isOnRope && Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = false;
            isOnRope = false;
            player.transform.SetParent(null);
            player.GetComponent<Rigidbody2D>().isKinematic = false;

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
