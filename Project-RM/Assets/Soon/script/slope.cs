using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slope : MonoBehaviour
{
    public Transform ropeStartPoint; // 로프 시작 위치
    public Transform ropeEndPoint;   // 로프 끝 위치
    public float moveSpeed = 2f;     // 로프 이동 속도
    public KeyCode interactKey = KeyCode.F; // 상호작용 키
    private bool isPlayerAttached = false;  // 플레이어가 로프에 붙었는지 여부
    public Transform player;               // 플레이어 트랜스폼
    private Vector3 targetPosition;         // 로프의 목표 위치
    private bool ropeStopped = true;       // 로프가 정지했는지 여부
    public bool canInteract = true;

    public Material outline;
    public Material noOutline;
    private Material myMaterial;

    public GameObject keyF;

    void Start()
    {
        canInteract = true;
        targetPosition = ropeEndPoint.position;
    }

    void Update()
    {
        if (isPlayerAttached)
        {
            // 로프가 움직이는 동안 플레이어를 고정
            player.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            

            // 로프가 정지하고 F 키를 누르면 로프에서 내림
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
            // 로프와 충돌 시 F 키를 눌러 로프에 탑승
            if (Input.GetKeyDown(interactKey) && IsPlayerInRange() && ropeStopped)
            {
                canInteract = false;
                AttachPlayer();
                ropeStopped = false;
                

            }
        }

        // 로프 이동
        if (!ropeStopped)
        {
            MoveRope();
        }
    }

    void MoveRope()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 목표 위치에 도달하면 정지
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ropeStopped = true;
        }
    }

    void AttachPlayer()
    {
        isPlayerAttached = true;
        player.GetComponent<PlayerController>().enabled = false; // 플레이어 조작 비활성화
    }

    void DetachPlayer()
    {
        isPlayerAttached = false;
        player.GetComponent<PlayerController>().enabled = true; // 플레이어 조작 활성화
        player = null; // 플레이어 참조 해제
        keyF.SetActive(false);
    }

    bool IsPlayerInRange()
    {
        // 플레이어와 로프 사이의 거리가 특정 범위 내인지 확인
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
