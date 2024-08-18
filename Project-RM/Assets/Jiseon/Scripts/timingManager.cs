using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>(); // ���� ��Ʈ��

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null; // Ÿ�̹� ��� ���� ������
    Vector2[] timingBoxs = null; // �������� �ִ�, �ּڰ�

    EffectManager theEffect;
    ComboManager theComboManager;
    NoteManager noteManager;
    CenterFrame centerFrame;
    public bool startMusic = false;

    private bool isHolding = false;
    private GameObject currentNote = null; // ���� ó�� ���� ��Ʈ
    GameObject endNote, curLongNote;
    public Transform notePos;

    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        noteManager = FindObjectOfType<NoteManager>();
        centerFrame = FindObjectOfType<CenterFrame>();

        // Ÿ�̹� �ڽ� ����
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2); // �������� = �ּҰ�, �ִ밪
        }

        theComboManager = FindObjectOfType<ComboManager>();
    }

    // ���� �޺� + 1 / Ȧ�� / �� �޺� + 1 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isHolding)
        {
            // Debug.Log("startlongnote");
            StartLongNote();
        }

        // �����̽��ٸ� ������ �� �ճ�Ʈ�� ����
        if (Input.GetKey(KeyCode.Space) && isHolding)
        {
        }

        // �����̽��ٸ� ������ �� �ճ�Ʈ�� ����
        if (Input.GetKeyUp(KeyCode.Space) && isHolding)
        {
            // Debug.Log("EndLongNote");
            EndLongNote();
        }
    }
    
    public void StartLongNote()
    {
        foreach (GameObject note in boxNoteList)
        {
            if (note.name == "LONGNOTE(Clone)")
            {
                GameObject middleNote = note.transform.GetChild(0).gameObject;
                GameObject startNote = note.transform.GetChild(1).gameObject;
                endNote = note.transform.GetChild(2).gameObject;
                curLongNote = note;
                if (CheckTiming(startNote))
                {
                    if (!centerFrame.musicStart && !startMusic)
                    {
                        centerFrame.MusicStart();
                        startMusic = true;
                    }
                    
                    isHolding = true;
                    startNote.SetActive(false);
                    currentNote = startNote;
                    theComboManager.IncreaseCombo();
                    startNote.transform.SetParent(note.transform);
                    startNote.transform.SetSiblingIndex(1);
                }
                else
                {
                    startNote.transform.SetParent(note.transform);
                    startNote.transform.SetSiblingIndex(1);
                    currentNote = null;
                }
                break;
            }
        }
    }

    void EndLongNote()
    {
        if (currentNote == null) return; 

        if (currentNote.activeSelf == false) // start note�� �ƴٸ�?
        {
            // Debug.Log("��ŸƮ ��Ʈ ����");
        }
        else // start long note �� �������ٸ�?
        {
            isHolding = false;
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // �̽� ó��
            noteManager.resetLongNote(endNote);
            currentNote = null;
            return;
        }
        

        if (CheckTiming(endNote))
        {
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            endNote.SetActive(false);
            theComboManager.IncreaseCombo();
            // theEffect.JudgementEffect(0); // ����Ʈ�� ����
        }
        else // �� ��Ʈ�� ���ƴٸ�?
        {

            // Debug.Log("���Ⱑ ����?");
            // EndNote ������ ������ ������ �̽� ó��
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            endNote.SetActive(false);

            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // �̽� ó��
        }

        noteManager.resetLongNote(endNote);
        currentNote = null;
    }

    bool CheckTiming(GameObject note)
    {
        if (note == null) {
            return false;
            }
        GameObject notes = note;

        note.transform.SetParent(notePos);

        float t_noteRecX = notes.transform.localPosition.x;

        for (int x = 0; x < 2; x++)
        {
            // Debug.Log(t_noteRecX + " , " + timingBoxs[x].x + " , " + timingBoxs[x].y);
            if (timingBoxs[x].x <= t_noteRecX && t_noteRecX <= timingBoxs[x].y)
            {
                theEffect.JudgementEffect(x);
                return true;
            }
        }
        return false;
    }

    public void CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            GameObject note = boxNoteList[i];

            float t_noteRecX = note.transform.localPosition.x;


            for (int x = 0; x < timingBoxs.Length; x++)
            {
                // Debug.Log(t_noteRecX + " , " + timingBoxs[x].x + " , " + timingBoxs[x].y);
                if (timingBoxs[x].x <= t_noteRecX && t_noteRecX <= timingBoxs[x].y) // NO
                {
                    Note noteComponent = note.GetComponent<Note>();

                    if (note.name == "LONGNOTE(Clone)")
                    {
                        // �ճ�Ʈ�� ����, �߰�, �� ��Ʈ�� �����Ͽ� ó��
                        GameObject startNote = note.transform.GetChild(1).gameObject;
                        GameObject endNote = note.transform.GetChild(2).gameObject;

                        if (CheckTiming(startNote))
                        {
                            // ���� ��Ʈ ����
                            // noteComponent.HideNote(); // ���� ��Ʈ ��Ȱ��ȭ
                            startNote.SetActive(false);
                            theComboManager.IncreaseCombo();
                            theEffect.JudgementEffect(x);
                        }
                        else if (CheckTiming(endNote))
                        {
                            // �� ��Ʈ ����
                            // noteComponent.HideNote(); // �� ��Ʈ ��Ȱ��ȭ
                            endNote.SetActive(false);
                            theComboManager.IncreaseCombo();
                            theEffect.JudgementEffect(x);
                        }
                        else
                        {
                            // �ճ�Ʈ�� ���� �Ǵ� ���� �ƴ� ���, �Ϲ� ��Ʈ�� �����ϰ� ó��
                            theComboManager.ResetCombo();
                            theEffect.JudgementEffect(timingBoxs.Length);
                        }

                        // ����Ʈ���� ����
                        boxNoteList.RemoveAt(i);
                        return;
                    }
                    else
                    {
                        // ���� ��Ʈ ó��
                        noteComponent.HideNote();
                        boxNoteList.RemoveAt(i);

                        if (x < timingBoxs.Length - 2)
                        {
                            theComboManager.IncreaseCombo();
                            theEffect.JudgementEffect(x);
                            theEffect.NoteHitEffect();
                        }
                        else if (x == 2)
                        {
                            theEffect.JudgementEffect(x);
                            theEffect.NoteHitEffect();
                            theComboManager.ResetCombo();
                        }
                        else if (x == 3)
                        {
                            theEffect.JudgementEffect(x);
                            theComboManager.ResetCombo();
                        }
                        return;
                    }
                }
            }
        }
        // Miss ó��
        theComboManager.ResetCombo();
        theEffect.JudgementEffect(timingBoxs.Length);
    }
}
