using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0; // �д� ��Ʈ �� �ǹ�
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear = null; // ��Ʈ ������ġ
    [SerializeField] RectTransform tfLongNoteAppear = null; // ��Ʈ ������ġ
    // [SerializeField] GameObject goNote = null; // ��Ʈ ������

    EffectManager effectManager;
    timingManager timingManager;
    ComboManager comboManager;

    private void Start()
    {
        effectManager = FindObjectOfType<EffectManager>();
        timingManager = GetComponent<timingManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    void Update()
    {
        NoteMaking();
    }

    public void NoteMaking()
    {
        currentTime += Time.deltaTime;
        double BeatTime = (60d / bpm) * 3;
        if (currentTime >= BeatTime) // 1��Ʈ�� �ð� 
        {
            MakeLongNote();
            currentTime -= BeatTime;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "StartNote" ||  collision.gameObject.name == "EndNote")
        {
            collision.gameObject.SetActive(false);

            if (collision.gameObject.name == "EndNote")
            {
                Note note = collision.GetComponent<Note>();
                note.enabled = false;

                GameObject parentGameObject = collision.transform.parent?.gameObject;

                if(parentGameObject != null)
                {
                    parentGameObject.transform.position = tfLongNoteAppear.position;
                    parentGameObject.gameObject.SetActive(false);
                    timingManager.boxNoteList.Remove(parentGameObject.gameObject);
                    ObjectPool.instance.longNoteQueue.Enqueue(parentGameObject.gameObject);


                }
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

    public void MakeNote()
    {
        GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
        t_note.transform.position = tfNoteAppear.position;
        t_note.SetActive(true);
        timingManager.boxNoteList.Add(t_note);
    }

    public void MakeLongNote()
    {
        GameObject t_note = ObjectPool.instance.longNoteQueue.Dequeue();
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
