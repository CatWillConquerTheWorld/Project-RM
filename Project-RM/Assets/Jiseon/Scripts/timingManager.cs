using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public static List<GameObject> boxNoteList = new List<GameObject>(); // 들어온 노트들

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null; // 타이밍 재는 기준 영역들
    Vector2[] timingBoxs = null; // 판정범위 최댓값, 최솟값

    EffectManager theEffect;
    ComboManager theComboManager;
    NoteManager noteManager;
    CenterFrame centerFrame;
    public bool startMusic = false;

    private bool isHolding = false;
    private GameObject currentNote = null; // 현재 처리 중인 노트
    GameObject endNote, curLongNote;
    public Transform notePos;

    public bool isLongNote = false;
    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        noteManager = FindObjectOfType<NoteManager>();
        centerFrame = FindObjectOfType<CenterFrame>();

        // 타이밍 박스 생성
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2); // 판정범위 = 최소값, 최대값
        }

        theComboManager = FindObjectOfType<ComboManager>();
    }

    // 시작 콤보 + 1 / 홀딩 / 끝 콤보 + 1 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && !isHolding)
        {
            // Debug.Log("startlongnote");
            StartLongNote();
        }

        // 스페이스바를 눌렀을 때 롱노트를 유지
        if (Input.GetKey(KeyCode.D) && isHolding)
        {
        }

        // 스페이스바를 떼었을 때 롱노트를 종료
        if (Input.GetKeyUp(KeyCode.D) && isHolding)
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
                isLongNote = true;
                GameObject middleNote = note.transform.GetChild(0).gameObject;
                GameObject startNote = note.transform.GetChild(1).gameObject;
                endNote = note.transform.GetChild(2).gameObject;
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

        if (currentNote.activeSelf == false) // start note가 됐다면?
        {
            // Debug.Log("스타트 노트 꺼짐");
        }
        else // start long note 를 못눌렀다면?
        {
            isHolding = false;
            theComboManager.ResetCombo();
            Debug.Log("스타트 노트 못누름 판정 리셋");
            theEffect.JudgementEffect(timingBoxs.Length); // 미스 처리
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
        else // 롱 노트를 놓쳤다면?
        {
            // Debug.Log("여기가 문제?");
            // EndNote 판정이 범위에 없으면 미스 처리
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            
            Debug.Log("롱노트 끝 판정 리셋");
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // 미스 처리
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
                return true;
            }
        }
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
                return true;
            }
        }
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
                            //// 롱노트의 시작, 중간, 끝 노트를 구분하여 처리
                            //GameObject startNote = note.transform.GetChild(1).gameObject;
                            //GameObject endNote = note.transform.GetChild(2).gameObject;

                            //if (CheckTiming(startNote))
                            //{
                            //    // 시작 노트 판정
                            //    // noteComponent.HideNote(); // 시작 노트 비활성화
                            //    startNote.SetActive(false);
                            //    theComboManager.IncreaseCombo();
                            //    theEffect.JudgementEffect(x);
                            //}
                            //else if (CheckTiming(endNote))
                            //{
                            //    // 끝 노트 판정
                            //    // noteComponent.HideNote(); // 끝 노트 비활성화
                            //    endNote.SetActive(false);
                            //    theComboManager.IncreaseCombo();
                            //    theEffect.JudgementEffect(x);
                            //}
                            //else
                            //{
                            //    // 롱노트의 시작 또는 끝이 아닐 경우, 일반 노트와 동일하게 처리
                            //    theComboManager.ResetCombo();
                            //    theEffect.JudgementEffect(timingBoxs.Length);
                            //}

                            //// 리스트에서 제거
                            //boxNoteList.RemoveAt(i);
                            //return;
                            return;
                        }
                        else
                        {
                            // 단일 노트 처리
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
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(1f);
                            }
                            else if (x == 2)
                            {
                                theComboManager.IncreaseCombo();
                                theEffect.JudgementEffect(x);
                                theEffect.NoteHitEffect();
                                //theComboManager.ResetCombo();
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(0.8f);
                            }
                            else if (x == 3)
                            {
                                theEffect.JudgementEffect(x);
                                theComboManager.ResetCombo();
                                GameObject.FindWithTag("Player").GetComponent<PlayerController>().ShortAttack(0.5f);
                            }
                            return;
                        }
                    }
                }
            }
            Debug.Log("단일노트 판정 리셋");
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length);
        }
        else
        {
            return;
        }
        
    }
}
