using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>(); // 들어온 노트들

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
        if (Input.GetKeyDown(KeyCode.Space) && !isHolding)
        {
            // Debug.Log("startlongnote");
            StartLongNote();
        }

        // 스페이스바를 눌렀을 때 롱노트를 유지
        if (Input.GetKey(KeyCode.Space) && isHolding)
        {
        }

        // 스페이스바를 떼었을 때 롱노트를 종료
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

        if (currentNote.activeSelf == false) // start note가 됐다면?
        {
            // Debug.Log("스타트 노트 꺼짐");
        }
        else // start long note 를 못눌렀다면?
        {
            isHolding = false;
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // 미스 처리
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
            // theEffect.JudgementEffect(0); // 퍼펙트로 가정
        }
        else // 롱 노트를 놓쳤다면?
        {

            // Debug.Log("여기가 문제?");
            // EndNote 판정이 범위에 없으면 미스 처리
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            endNote.SetActive(false);

            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // 미스 처리
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
                        // 롱노트의 시작, 중간, 끝 노트를 구분하여 처리
                        GameObject startNote = note.transform.GetChild(1).gameObject;
                        GameObject endNote = note.transform.GetChild(2).gameObject;

                        if (CheckTiming(startNote))
                        {
                            // 시작 노트 판정
                            // noteComponent.HideNote(); // 시작 노트 비활성화
                            startNote.SetActive(false);
                            theComboManager.IncreaseCombo();
                            theEffect.JudgementEffect(x);
                        }
                        else if (CheckTiming(endNote))
                        {
                            // 끝 노트 판정
                            // noteComponent.HideNote(); // 끝 노트 비활성화
                            endNote.SetActive(false);
                            theComboManager.IncreaseCombo();
                            theEffect.JudgementEffect(x);
                        }
                        else
                        {
                            // 롱노트의 시작 또는 끝이 아닐 경우, 일반 노트와 동일하게 처리
                            theComboManager.ResetCombo();
                            theEffect.JudgementEffect(timingBoxs.Length);
                        }

                        // 리스트에서 제거
                        boxNoteList.RemoveAt(i);
                        return;
                    }
                    else
                    {
                        // 단일 노트 처리
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
        // Miss 처리
        theComboManager.ResetCombo();
        theEffect.JudgementEffect(timingBoxs.Length);
    }
}
