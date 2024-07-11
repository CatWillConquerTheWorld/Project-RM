using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>(); // ���� ��Ʈ��

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null; // Ÿ�̹� ��� ���� ������
    Vector2[] timingBoxs = null; // �������� �ִ�, �ּڰ�

    EffectManager theEffect;
    ComboManager theComboManager;

    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        // Ÿ�̹� �ڽ� ����
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2); // �������� = �ּҰ�, �ִ밪
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

                    if(x < timingBoxs.Length - 2) // 0~1�� ���� . 0 - ����Ʈ , 1 - ��
                    {
                        theComboManager.IncreaseCombo();
                        theEffect.JudgementEffect(x);
                        theEffect.NoteHitEffect();
                        return;
                    } else if (x == 2) // 2 - �� 
                    {
                        theEffect.JudgementEffect(x);
                        theEffect.NoteHitEffect();
                        theComboManager.ResetCombo();
                        return;
                    }
                    else if(x == 3)
                    {
                        theEffect.JudgementEffect(x); // 3- ���
                        theComboManager.ResetCombo();
                        return;
                    }
                    
                }
            }
        }
        theComboManager.ResetCombo(); // 4 - �̽�
        theEffect.JudgementEffect(timingBoxs.Length);
        // Debug.Log("Miss"); // ������ �����������.
    }
}
