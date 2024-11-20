using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slope : MonoBehaviour
{
    public Transform ropeStartPoint; // ���� ���� ��ġ
    public Transform ropeEndPoint;   // ���� �� ��ġ
    public float moveSpeed = 2f;     // ���� �̵� �ӵ�
    public KeyCode interactKey = KeyCode.F; // ��ȣ�ۿ� Ű
    private bool isPlayerAttached = false;  // �÷��̾ ������ �پ����� ����
    public Transform player;               // �÷��̾� Ʈ������
    private Vector3 targetPosition;         // ������ ��ǥ ��ġ
    private bool ropeStopped = true;       // ������ �����ߴ��� ����

    void Start()
    {
        targetPosition = ropeEndPoint.position;
    }

    void Update()
    {
        if (isPlayerAttached)
        {
            // ������ �����̴� ���� �÷��̾ ����
            player.position = new Vector3(transform.position.x, transform.position.y - 1.1f, transform.position.z);

            // ������ �����ϰ� F Ű�� ������ �������� ����
            if (ropeStopped && Input.GetKeyDown(interactKey))
            {
                DetachPlayer();
            }
        }
        else
        {
            // ������ �浹 �� F Ű�� ���� ������ ž��
            if (Input.GetKeyDown(interactKey) && IsPlayerInRange() && ropeStopped)
            {
                AttachPlayer();
                ropeStopped = false;
            }
        }

        // ���� �̵�
        if (!ropeStopped)
        {
            MoveRope();
        }
    }

    void MoveRope()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ��ǥ ��ġ�� �����ϸ� ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ropeStopped = true;
        }
    }

    void AttachPlayer()
    {
        isPlayerAttached = true;
        player.GetComponent<PlayerController>().enabled = false; // �÷��̾� ���� ��Ȱ��ȭ
    }

    void DetachPlayer()
    {
        isPlayerAttached = false;
        player.GetComponent<PlayerController>().enabled = true; // �÷��̾� ���� Ȱ��ȭ
        player = null; // �÷��̾� ���� ����
    }

    bool IsPlayerInRange()
    {
        // �÷��̾�� ���� ������ �Ÿ��� Ư�� ���� ������ Ȯ��
        return Vector3.Distance(player.position, transform.position) < 2f;
    }

    
}
