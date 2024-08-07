using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Animator portalAnimator;
    private float timeOnPortal = 0f; // ��Ż A ���� �ִ� �ð�
    private bool isOnPortal = false; // �÷��̾ ��Ż A ���� �ִ��� ����
    

    void Update()
    {
        if (isOnPortal)
        {
            timeOnPortal += Time.deltaTime;

            if (timeOnPortal >= 3f) // 3�� �̻� �ӹ����� ��
            {
                bool isTeleport = portalAnimator.GetBool("isTeleport");
                portalAnimator.SetBool("isTeleport", !isTeleport);
                timeOnPortal = 0f; // �ð��� �ʱ�ȭ�մϴ�.
                isOnPortal = false; // ���¸� �ʱ�ȭ�մϴ�.
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ��Ż�� ������ ��
        {
            isOnPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ��Ż���� ������ ��
        {
            isOnPortal = false;
            timeOnPortal = 0f; // �ð��� �ʱ�ȭ�մϴ�.
        }
    }

}
