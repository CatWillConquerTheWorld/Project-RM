using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class longnote3 : MonoBehaviour
{
    public GameObject startNote, endNote;
    public RectTransform startUI;
    public RectTransform endUI; // 끝 UI 오브젝트
    private RectTransform line;
    public GameObject lineObject;
    public Image lineImg;
    public RectTransform lineRect;

    void Start()
    {
        // lineImg.color = Color.black; // 선의 색상 설정
        startNote.SetActive(true);
        endNote.SetActive(true);
        lineObject.SetActive(true);
        lineRect.SetParent(transform, false); // 부모 설정
        lineRect.localPosition = Vector3.zero; // 로컬 위치를 (0, 0)으로 초기화
    }

    // Update is called once per frame
    void Update()
    {
        Resize();
        if (startNote.activeSelf == false && endNote.activeSelf == false)
        {
            lineObject.SetActive(false);
            gameObject.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartEndNote();
        }

    }


    public void Resize()
    {
        Vector3 startPos = startUI.position;
        Vector3 endPos = endUI.position;

        float distance = Vector3.Distance(startPos, endPos);
        lineRect.sizeDelta = new Vector2(distance, 80f); // 선의 길이와 두께 설정

        // 선의 중앙 위치 설정
        lineRect.position = (startPos + endPos) / 2;

        // 선의 회전 설정 (세로 방향으로)
        float angle = Vector3.Angle(Vector3.right, endPos - startPos);
        if (endPos.y < startPos.y) angle = -angle; // 방향에 따라 각도 조정
        lineRect.rotation = Quaternion.Euler(0, 0, angle); // 각도 설정
    }


    public void StartEndNote()
    {
        Note note = endNote.GetComponent<Note>();
        note.enabled = true;
    }
}
