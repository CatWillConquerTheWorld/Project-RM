using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform movePortal; // ��Ż B�� Transform�� �����մϴ�.
    public float plusY = 1f;
    

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            
            Vector3 newPosition = movePortal.position;
            newPosition.y += plusY;
            player.transform.position = newPosition; // �÷��̾ ��Ż B ��ġ�� �̵�
            
        }
    }
}
