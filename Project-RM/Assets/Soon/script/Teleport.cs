using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform movePortal; // 포탈 B의 Transform을 연결합니다.
    public float plusY = 1f;
    

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            
            Vector3 newPosition = movePortal.position;
            newPosition.y += plusY;
            player.transform.position = newPosition; // 플레이어를 포탈 B 위치로 이동
            
        }
    }
}
