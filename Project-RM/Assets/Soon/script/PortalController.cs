using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Animator portalAnimator;
    private float timeOnPortal = 0f; // 포탈 A 위에 있는 시간
    private bool isOnPortal = false; // 플레이어가 포탈 A 위에 있는지 여부
    

    void Update()
    {
        if (isOnPortal)
        {
            timeOnPortal += Time.deltaTime;

            if (timeOnPortal >= 3f) // 3초 이상 머물렀을 때
            {
                bool isTeleport = portalAnimator.GetBool("isTeleport");
                portalAnimator.SetBool("isTeleport", !isTeleport);
                timeOnPortal = 0f; // 시간을 초기화합니다.
                isOnPortal = false; // 상태를 초기화합니다.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 포탈에 들어왔을 때
        {
            isOnPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 포탈에서 나갔을 때
        {
            isOnPortal = false;
            timeOnPortal = 0f; // 시간을 초기화합니다.
        }
    }

}
