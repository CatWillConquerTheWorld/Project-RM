
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm; // 분당 비트 수 의미
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear = null; // 노트 생성위치
    [SerializeField] RectTransform tfLongNoteAppear = null; // 노트 생성위치
    // [SerializeField] GameObject goNote = null; // 노트 프리팹

    EffectManager effectManager;
    public timingManager timingManager;
    ComboManager comboManager;

    bool notecheck = false;

    public static bool isMusicStarted;

    private void Start()
    {
        isMusicStarted = false;
        bpm = bpmManager.instance.bpm;
        effectManager = FindObjectOfType<EffectManager>();
        // timingManager = GetComponent<timingManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    void Update()
    {
        //if (isMusicStarted)
        //{
        //    NoteMaking();
        //}
        if (!isMusicStarted) currentTime = 0f;
    }
    GameObject curLongNote;
    longnote3 curLongNoteScript;
    bool is_longnote_start = false;
    public void NoteMaker(string note)
    {
        // Debug.Log(note);
        if (note == "AA")
        {
            MakeNote();

        }
        else if(note == "AB")
        {
            if (!is_longnote_start)
            { // 롱노트 시작부분이라면
                MakeLongNote();
                is_longnote_start = true;
                // Debug.Log("long note start");
            }
            else
            {
                is_longnote_start = false;
                curLongNoteScript.StartEndNote();
                // Debug.Log("long note end");
                curLongNote = null;
                curLongNoteScript = null;
            }
        }else if(note == "00"){

        }
    }

    public void NoteMaking()
    {
        currentTime += Time.deltaTime;
        double BeatTime = bpmManager.instance.bpmInterval * 3;
        if (currentTime >= BeatTime && gameObject.activeSelf) // 1비트의 시간 
        {
            if(notecheck)
            {
                MakeLongNote();
                notecheck = false;
            }
            else
            {
                MakeNote();
                notecheck = true;
            }
            currentTime -= BeatTime;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.gameObject.activeSelf) { return;  }

        if (collision.gameObject.name == "StartNote" ||  collision.gameObject.name == "EndNote")
        {
            collision.gameObject.SetActive(false);

            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                effectManager.JudgementEffect(4);
                timingManager.isLongNote = false;
                
                if(collision.gameObject.name == "StartNote")
                {
                    comboManager.ResetCombo();
                }
                // comboManager.ResetCombo();
                
            }

            if (collision.gameObject.name == "EndNote")
            {
                Note note = collision.GetComponent<Note>();
                note.enabled = false;

                GameObject parentGameObject = collision.transform.parent?.gameObject;

                if(parentGameObject != null && parentGameObject.activeSelf )
                {
                    parentGameObject.transform.position = tfLongNoteAppear.position;
                    if (parentGameObject.activeSelf)  // 활성화 상태 확인
                    {
                        parentGameObject.gameObject.SetActive(false);
                    }
                    timingManager.boxNoteList.Remove(parentGameObject.gameObject);
                    ObjectPool.instance.longNoteQueue.Enqueue(parentGameObject.gameObject);
                }

                comboManager.ResetCombo();
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().LongAttackCancel(); // 얘 나중에 다시 주석풀기
            }
        }
        else
        {
            if (collision.CompareTag("Note"))
            {
                if (collision.GetComponent<Note>().GetNoteFlag())
                {
                    effectManager.JudgementEffect(4);
                    comboManager.ResetCombo();
                }

                timingManager.boxNoteList.Remove(collision.gameObject);
                ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
                collision.gameObject.SetActive(false);

            }
        }
    }

    public void resetLongNote(GameObject longNote)
    {
        Note note = longNote.GetComponent<Note>();
        note.enabled = false;
        longNote.SetActive(false);

        GameObject parentGameObject = longNote.transform.parent?.gameObject;

        if (parentGameObject != null)
        {
            parentGameObject.transform.position = tfLongNoteAppear.position;
            parentGameObject.gameObject.SetActive(false);
            timingManager.boxNoteList.Remove(parentGameObject.gameObject);
            ObjectPool.instance.longNoteQueue.Enqueue(parentGameObject.gameObject);
        }
    }

    public void finishLongNote(GameObject longNote)
    {
        if (longNote.gameObject.name == "StartNote" || longNote.gameObject.name == "EndNote")
        {
            longNote.gameObject.SetActive(false);

            if (longNote.gameObject.name == "EndNote")
            {
                Note note = longNote.GetComponent<Note>();
                note.enabled = false;

                GameObject parentGameObject = longNote.transform.parent?.gameObject;

                if (parentGameObject != null && parentGameObject.activeSelf)
                {
                    parentGameObject.transform.position = tfLongNoteAppear.position;
                    parentGameObject.gameObject.SetActive(false);
                    timingManager.boxNoteList.Remove(parentGameObject.gameObject);
                    ObjectPool.instance.longNoteQueue.Enqueue(parentGameObject.gameObject);
                }
            }
        }
    }

    public void MakeNote()
    {
        GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
        Note note = t_note.GetComponent<Note>();
        
        t_note.transform.position = tfNoteAppear.position;
        t_note.SetActive(true);
        note.noteDir = Vector3.right;
        timingManager.boxNoteList.Add(t_note);
    }

    public void MakeLongNote()
    {
        GameObject t_note = ObjectPool.instance.longNoteQueue.Dequeue();
        curLongNote = t_note;
        curLongNoteScript = curLongNote.GetComponent<longnote3>();
        RectTransform rect = t_note.GetComponent<RectTransform>();
        rect.position = tfLongNoteAppear.position;
        t_note.SetActive(true);
        longnote3 longnote3 = t_note.GetComponent<longnote3>();

        longnote3.isNoteActive = true;
        longnote3.lineObject.SetActive(true);
        longnote3.startNote.SetActive(true);
        longnote3.endNote.SetActive(true);

        longnote3.startNote.transform.position = tfNoteAppear.position;
        longnote3.endNote.transform.position = tfNoteAppear.position;
        longnote3.FinishEndNote();
        timingManager.boxNoteList.Add(t_note);
    }
}
