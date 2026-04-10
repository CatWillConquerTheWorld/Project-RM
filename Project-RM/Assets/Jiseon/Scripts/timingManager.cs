using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public static List<GameObject> boxNoteList = new List<GameObject>(); // ���� ��Ʈ��

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

    public bool isLongNote = false;
    public bool emptyNoteBoxListCheck = false;
    public bool longNoteFail = false;
    public Animator UIGunAnimator;

    private float lastInputTime = 0f; // ���������� �Է� ó���� �ð�
    public float inputCooldown = 0.1f; // �ּ� �Է� ���� (��)
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
        if (LoadBMS.Instance.noteNum == 0 && !emptyNoteBoxListCheck)
        {
            clearBoxNoteList();
            emptyNoteBoxListCheck = true;
        }
        else if (LoadBMS.Instance.noteNum != 0)
        {
            emptyNoteBoxListCheck = false;
        }

        if (Input.GetKeyDown(KeyCode.D) && !isHolding && !longNoteFail)
        {
            if (Time.time - lastInputTime >= inputCooldown)
            {
                lastInputTime = Time.time; // ������ �Է� �ð� ����
                StartLongNote();
            }
        }

        // �����̽��ٸ� ������ �� �ճ�Ʈ�� ����
        if (Input.GetKey(KeyCode.D) && isHolding)
        {
        }

        if (Input.GetKeyUp(KeyCode.D) && isHolding)
        {
            if (Time.time - lastInputTime >= inputCooldown)
            {
                lastInputTime = Time.time; // ������ �Է� �ð� ����
                EndLongNote();
            }
        }

        if(centerFrame.music_change == true)
        {
            foreach (GameObject note in boxNoteList)
            {
                Destroy(note);
            }

            // ����Ʈ�� ����
            boxNoteList.Clear();
            centerFrame.music_change = false;
        }
    }
    
    public void clearBoxNoteList()
    {
        foreach (GameObject note in boxNoteList)
        {
            Destroy(note);
        }

        // ����Ʈ�� ����
        boxNoteList.Clear();
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


                isLongNote = true;
                curLongNote = note;
                if (CheckTiming(startNote))
                {
                    if (!CenterFrame.musicStart && !startMusic)
                    {
                        Debug.Log("music start long note");
                        // centerFrame.MusicStart();
                        startMusic = true;
                    }
                    isHolding = true;
                    startNote.transform.position = centerFrame.transform.position;
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
            Debug.Log("��ŸƮ ��Ʈ ������ ���� ����");
            theEffect.JudgementEffect(timingBoxs.Length); // �̽� ó��
            noteManager.resetLongNote(endNote);
            currentNote = null;
            return;
        }
        

        if (CheckTiming2(endNote))
        {
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            endNote.SetActive(false);
            theComboManager.IncreaseCombo();
        }
        else // �� ��Ʈ�� ���ƴٸ�?
        {
            // Debug.Log("���Ⱑ ����?");
            // EndNote ������ ������ ������ �̽� ó��
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            
            Debug.Log("�ճ�Ʈ �� ���� ����");
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // �̽� ó��
            // noteManager.resetLongNote(endNote);
        }
        isLongNote = false;
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

        for (int x = 0; x < 3; x++)
        {
            // Debug.Log(t_noteRecX + " , " + timingBoxs[x].x + " , " + timingBoxs[x].y);
            if (timingBoxs[x].x <= t_noteRecX && t_noteRecX <= timingBoxs[x].y)
            {
                if (x == 0) GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttackPrepare(1.5f);
                else if (x == 1) GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttackPrepare(1f);
                else if (x == 2) GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttackPrepare(0.8f);
                theEffect.JudgementEffect(x);
                theEffect.NoteHitEffect();
                UIGunAnimator.SetTrigger("LongShoot");
                return true;
            }
        }
        UIGunAnimator.SetTrigger("back");
        return false;
    }

    bool CheckTiming2(GameObject note)
    {
        if (note == null)
        {
            return false;
        }
        GameObject notes = note;

        note.transform.SetParent(notePos);

        float t_noteRecX = notes.transform.localPosition.x;

        for (int x = 0; x < 3; x++)
        {
            // Debug.Log(t_noteRecX + " , " + timingBoxs[x].x + " , " + timingBoxs[x].y);
            if (timingBoxs[x].x <= t_noteRecX && t_noteRecX <= timingBoxs[x].y)
            {
                if (x == 0) GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttack(1.5f);
                else if (x == 1) GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttack(1f);
                else if (x == 2) GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttack(0.8f);
                theEffect.JudgementEffect(x);
                theEffect.NoteHitEffect();
                UIGunAnimator.SetTrigger("Shoot");
                return true;
            }
        }
        UIGunAnimator.SetTrigger("back");
        return false;
    }

    public void CheckTiming()
    {
        if (isLongNote == false)
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
                        // Debug.Log(note.gameObject.name);
                        if (note.gameObject.name != "Note(Clone)")
                        {
                            //// �ճ�Ʈ�� ����, �߰�, �� ��Ʈ�� �����Ͽ� ó��
                            //GameObject startNote = note.transform.GetChild(1).gameObject;
                            //GameObject endNote = note.transform.GetChild(2).gameObject;

                            //if (CheckTiming(startNote))
                            //{
                            //    // ���� ��Ʈ ����
                            //    // noteComponent.HideNote(); // ���� ��Ʈ ��Ȱ��ȭ
                            //    startNote.SetActive(false);
                            //    theComboManager.IncreaseCombo();
                            //    theEffect.JudgementEffect(x);
                            //}
                            //else if (CheckTiming(endNote))
                            //{
                            //    // �� ��Ʈ ����
                            //    // noteComponent.HideNote(); // �� ��Ʈ ��Ȱ��ȭ
                            //    endNote.SetActive(false);
                            //    theComboManager.IncreaseCombo();
                            //    theEffect.JudgementEffect(x);
                            //}
                            //else
                            //{
                            //    // �ճ�Ʈ�� ���� �Ǵ� ���� �ƴ� ���, �Ϲ� ��Ʈ�� �����ϰ� ó��
                            //    theComboManager.ResetCombo();
                            //    theEffect.JudgementEffect(timingBoxs.Length);
                            //}

                            //// ����Ʈ���� ����
                            //boxNoteList.RemoveAt(i);
                            //return;
                            return;
                        }
                        else
                        {
                            // ���� ��Ʈ ó��
                            noteComponent.HideNote();
                            boxNoteList.RemoveAt(i);

                            if (x == 0)
                            {
                                theComboManager.IncreaseCombo();
                                theEffect.JudgementEffect(x);
                                theEffect.NoteHitEffect();
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(1.5f);
                            }
                            else if (x == 1)
                            {
                                theComboManager.IncreaseCombo();
                                theEffect.JudgementEffect(x);
                                theEffect.NoteHitEffect();
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(1.25f);
                            }
                            else if (x == 2)
                            {
                                theComboManager.IncreaseCombo();
                                theEffect.JudgementEffect(x);
                                theEffect.NoteHitEffect();
                                //theComboManager.ResetCombo();
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(1f);
                            }
                            else if (x == 3)
                            {
                                theEffect.JudgementEffect(x);
                                theComboManager.ResetCombo();
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(0.75f);
                            }
                            UIGunAnimator.SetTrigger("Shoot");
                            return;
                        }
                    }
                }
            }
            Debug.Log("���ϳ�Ʈ ���� ����");
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length);
            UIGunAnimator.SetTrigger("back");
        }
        else
        {
            return;
        }
        
    }
}
