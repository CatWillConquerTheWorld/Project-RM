using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class timingManager : MonoBehaviour
{
    public static List<GameObject> boxNoteList = new List<GameObject>(); // 占쏙옙占쏙옙 占쏙옙트占쏙옙

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null; // 타占싱뱄옙 占쏙옙占?占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙
    Vector2[] timingBoxs = null; // 占쏙옙占쏙옙占쏙옙占쏙옙 占쌍댐옙, 占쌍솟곤옙

    EffectManager theEffect;
    ComboManager theComboManager;
    NoteManager noteManager;
    CenterFrame centerFrame;
    public bool startMusic = false;

    private bool isHolding = false;
    private GameObject currentNote = null; // 占쏙옙占쏙옙 처占쏙옙 占쏙옙占쏙옙 占쏙옙트
    GameObject endNote, curLongNote;
    public Transform notePos;

    public bool isLongNote = false;
    public bool emptyNoteBoxListCheck = false;
    public bool longNoteFail = false;
    public Animator UIGunAnimator;

    private float lastInputTime = 0f; // 占쏙옙占쏙옙占쏙옙占쏙옙占쏙옙 占쌉뤄옙 처占쏙옙占쏙옙 占시곤옙
    public float inputCooldown = 0.1f; // 占쌍쇽옙 占쌉뤄옙 占쏙옙占쏙옙 (占쏙옙)
    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        noteManager = FindObjectOfType<NoteManager>();
        centerFrame = FindObjectOfType<CenterFrame>();

        // 타占싱뱄옙 占쌘쏙옙 占쏙옙占쏙옙
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2); // 占쏙옙占쏙옙占쏙옙占쏙옙 = 占쌍소곤옙, 占쌍대값
        }

        theComboManager = FindObjectOfType<ComboManager>();
    }

    // 占쏙옙占쏙옙 占쌨븝옙 + 1 / 홀占쏙옙 / 占쏙옙 占쌨븝옙 + 1 
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
                lastInputTime = Time.time; // 占쏙옙占쏙옙占쏙옙 占쌉뤄옙 占시곤옙 占쏙옙占쏙옙
                StartLongNote();
            }
        }

        // 占쏙옙占쏙옙占싱쏙옙占쌕몌옙 占쏙옙占쏙옙占쏙옙 占쏙옙 占쌌놂옙트占쏙옙 占쏙옙占쏙옙
        if (Input.GetKey(KeyCode.D) && isHolding)
        {
        }

        if (Input.GetKeyUp(KeyCode.D) && isHolding)
        {
            if (Time.time - lastInputTime >= inputCooldown)
            {
                lastInputTime = Time.time; // 占쏙옙占쏙옙占쏙옙 占쌉뤄옙 占시곤옙 占쏙옙占쏙옙
                EndLongNote();
            }
        }

        if(centerFrame.music_change == true)
        {
            clearBoxNoteList();
            centerFrame.music_change = false;
        }
    }
    
    public void clearBoxNoteList()
    {
        foreach (GameObject note in boxNoteList)
        {
            if (note == null)
            {
                continue;
            }

            if (note.name == "LONGNOTE(Clone)")
            {
                longnote3 longNote = note.GetComponent<longnote3>();
                if (longNote != null && ObjectPool.instance != null)
                {
                    ObjectPool.instance.ReturnLongNote(note, longNote.isEnemyLongNote);
                }
                else
                {
                    note.SetActive(false);
                }
            }
            else
            {
                note.SetActive(false);
                if (ObjectPool.instance != null)
                {
                    ObjectPool.instance.noteQueue.Enqueue(note);
                }
            }
        }

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

        if (currentNote.activeSelf == false) // start note占쏙옙 占싣다몌옙?
        {
            // Debug.Log("占쏙옙타트 占쏙옙트 占쏙옙占쏙옙");
        }
        else // start long note 占쏙옙 占쏙옙占쏙옙占쏙옙占쌕몌옙?
        {
            isHolding = false;
            theComboManager.ResetCombo();
            Debug.Log("占쏙옙타트 占쏙옙트 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙");
            theEffect.JudgementEffect(timingBoxs.Length); // 占싱쏙옙 처占쏙옙
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
        else // 占쏙옙 占쏙옙트占쏙옙 占쏙옙占싣다몌옙?
        {
            // Debug.Log("占쏙옙占썩가 占쏙옙占쏙옙?");
            // EndNote 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占싱쏙옙 처占쏙옙
            endNote.transform.SetParent(curLongNote.transform);
            endNote.transform.SetSiblingIndex(2);
            isHolding = false;
            
            Debug.Log("占쌌놂옙트 占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙");
            theComboManager.ResetCombo();
            theEffect.JudgementEffect(timingBoxs.Length); // 占싱쏙옙 처占쏙옙
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
                            //// 占쌌놂옙트占쏙옙 占쏙옙占쏙옙, 占쌩곤옙, 占쏙옙 占쏙옙트占쏙옙 占쏙옙占쏙옙占싹울옙 처占쏙옙
                            //GameObject startNote = note.transform.GetChild(1).gameObject;
                            //GameObject endNote = note.transform.GetChild(2).gameObject;

                            //if (CheckTiming(startNote))
                            //{
                            //    // 占쏙옙占쏙옙 占쏙옙트 占쏙옙占쏙옙
                            //    // noteComponent.HideNote(); // 占쏙옙占쏙옙 占쏙옙트 占쏙옙활占쏙옙화
                            //    startNote.SetActive(false);
                            //    theComboManager.IncreaseCombo();
                            //    theEffect.JudgementEffect(x);
                            //}
                            //else if (CheckTiming(endNote))
                            //{
                            //    // 占쏙옙 占쏙옙트 占쏙옙占쏙옙
                            //    // noteComponent.HideNote(); // 占쏙옙 占쏙옙트 占쏙옙활占쏙옙화
                            //    endNote.SetActive(false);
                            //    theComboManager.IncreaseCombo();
                            //    theEffect.JudgementEffect(x);
                            //}
                            //else
                            //{
                            //    // 占쌌놂옙트占쏙옙 占쏙옙占쏙옙 占실댐옙 占쏙옙占쏙옙 占싣댐옙 占쏙옙占? 占싹뱄옙 占쏙옙트占쏙옙 占쏙옙占쏙옙占싹곤옙 처占쏙옙
                            //    theComboManager.ResetCombo();
                            //    theEffect.JudgementEffect(timingBoxs.Length);
                            //}

                            //// 占쏙옙占쏙옙트占쏙옙占쏙옙 占쏙옙占쏙옙
                            //boxNoteList.RemoveAt(i);
                            //return;
                            return;
                        }
                        else
                        {
                            // 占쏙옙占쏙옙 占쏙옙트 처占쏙옙
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
            Debug.Log("占쏙옙占싹놂옙트 占쏙옙占쏙옙 占쏙옙占쏙옙");
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

