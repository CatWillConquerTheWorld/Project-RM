using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>(); // 들어온 노트들

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null; // 타이밍 재는 기준 영역들
    Vector2[] timingBoxs = null; // 판정범위 최댓값, 최솟값

    void Start()
    {
        // 타이밍 박스 생성
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2); // 판정범위 = 최소값, 최대값
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckTiming()
    {
        for(int i = 0; i < boxNoteList.Count; i++)
        {
            float t_noteRecX = boxNoteList[i].transform.localPosition.x;

            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= t_noteRecX && t_noteRecX  <= timingBoxs[x].y)
                {
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    switch (x)
                    {
                        case 0:
                            Debug.Log("Critical");
                            break;
                        case 1:
                            Debug.Log("Hit!");
                            break;
                        case 2:
                            Debug.Log("Bad");
                            break;
                        default :
                            Debug.Log("Miss");
                            break;

                    }
                    return;
                }
            }
        }

        // Debug.Log("Miss"); // 완전히 빗나갔을경우.
    }
}
