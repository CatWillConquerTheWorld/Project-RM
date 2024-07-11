using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>(); // 들어온 노트들

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null; // 타이밍 재는 기준 영역들
    Vector2[] timingBoxs = null; // 판정범위 최댓값, 최솟값

    EffectManager theEffect;
    ComboManager theComboManager;

    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        // 타이밍 박스 생성
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2); // 판정범위 = 최소값, 최대값
        }

        theComboManager = FindObjectOfType<ComboManager>();
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

                    if(x < timingBoxs.Length - 2) // 0~1만 나옴 . 0 - 퍼펙트 , 1 - 쿨
                    {
                        theComboManager.IncreaseCombo();
                        theEffect.JudgementEffect(x);
                        theEffect.NoteHitEffect();
                        return;
                    } else if (x == 2) // 2 - 굳 
                    {
                        theEffect.JudgementEffect(x);
                        theEffect.NoteHitEffect();
                        theComboManager.ResetCombo();
                        return;
                    }
                    else if(x == 3)
                    {
                        theEffect.JudgementEffect(x); // 3- 배드
                        theComboManager.ResetCombo();
                        return;
                    }
                    
                }
            }
        }
        theComboManager.ResetCombo(); // 4 - 미스
        theEffect.JudgementEffect(timingBoxs.Length);
        // Debug.Log("Miss"); // 완전히 빗나갔을경우.
    }
}
