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
    public bool canInteract = true;

    public Material outline;
    public Material noOutline;
    private Material myMaterial;

    public GameObject keyF;

    public AudioSource chain;

    void Start()
    {
        canInteract = true;
        targetPosition = ropeEndPoint.position;
    }

    void Update()
    {
        if (isPlayerAttached)
        {
            

            // ������ �����ϰ� F Ű�� ������ �������� ����
            if (ropeStopped)
                keyF.SetActive(true);
            if (ropeStopped && Input.GetKeyDown(interactKey))
            {
                DetachPlayer();
                keyF.SetActive(false);
            }
        }
        else
        {
            // ������ �浹 �� F Ű�� ���� ������ ž��
            if (Input.GetKeyDown(interactKey) && IsPlayerInRange() && ropeStopped)
            {
                canInteract = false;
                AttachPlayer();
                ropeStopped = false;
                chain.Play();


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
            chain.Stop();
        }
    }

    void AttachPlayer()
    {
        isPlayerAttached = true;
        player.SetParent(transform); // ������ �θ�� ����
        player.GetComponent<PlayerController>().enabled = false;
        player.position = new Vector3(16.5f, -6.8f, 0f);

        player.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void DetachPlayer()
    {
        isPlayerAttached = false;
        player.SetParent(null); // �θ� ���� ����
        player.GetComponent<PlayerController>().enabled = true; // �÷��̾� ���� Ȱ��ȭ
        player.GetComponent<Rigidbody2D>().gravityScale = 3;
        keyF.SetActive(false);
    }

    bool IsPlayerInRange()
    {
        // �÷��̾�� ���� ������ �Ÿ��� Ư�� ���� ������ Ȯ��
        return Vector3.Distance(player.position, transform.position) < 2f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canInteract && !ropeStopped)
        {
            keyF.SetActive(false);
            GetComponent<SpriteRenderer>().material = noOutline;
        }           
        else if (collision.tag == "Player" && Stage2.clearChap1 && canInteract)
        {
            keyF.SetActive(true);
            GetComponent<SpriteRenderer>().material = outline;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Stage2.clearChap1)
        {
            keyF.SetActive(false);
            GetComponent<SpriteRenderer>().material = noOutline;
        }
    }
}
