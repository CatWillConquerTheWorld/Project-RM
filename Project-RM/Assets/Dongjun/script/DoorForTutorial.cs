using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorForTutorial : MonoBehaviour
{
    public Animator animator; // 애니메이터를 연결합니다.
    private BoxCollider2D boxCollider; // 문 오브젝트에 있는 BoxCollider2D를 가져옵니다.

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // F 키를 누르면
        {
            bool isOpen = animator.GetBool("isOpen");
            animator.SetBool("isOpen", !isOpen); // isOpen 파라미터를 토글하여 문을 열고 닫음
            boxCollider.enabled = isOpen; // 문이 열리면 콜라이더를 비활성화하여 지나갈 수 있게 함
        }
    }

    IEnumerator OpenDoor()
    {

    }
}
