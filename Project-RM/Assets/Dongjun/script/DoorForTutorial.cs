using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorForTutorial : MonoBehaviour
{
    public Animator animator; // �ִϸ����͸� �����մϴ�.
    private BoxCollider2D boxCollider; // �� ������Ʈ�� �ִ� BoxCollider2D�� �����ɴϴ�.

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // F Ű�� ������
        {
            bool isOpen = animator.GetBool("isOpen");
            animator.SetBool("isOpen", !isOpen); // isOpen �Ķ���͸� ����Ͽ� ���� ���� ����
            boxCollider.enabled = isOpen; // ���� ������ �ݶ��̴��� ��Ȱ��ȭ�Ͽ� ������ �� �ְ� ��
        }
    }

    IEnumerator OpenDoor()
    {

    }
}
