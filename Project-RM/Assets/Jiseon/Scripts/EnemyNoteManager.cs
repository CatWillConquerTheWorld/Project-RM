using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    public int bpm = 0; // �д� ��Ʈ �� �ǹ�
    double currentTime = 0d;

    [SerializeField] Transform tfEnemyNoteAppear = null; // ��Ʈ ������ġ
    [SerializeField] RectTransform tfEnemyLongNoteAppear = null; // ��Ʈ ������ġ

    bool notecheck = false; // long short �����ư��� �������.
    

    void Start()
    {
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
            if (notecheck)
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
        if (collision.gameObject.name == "StartNote" || collision.gameObject.name == "EndNote")
        {
            collision.gameObject.SetActive(false);            

            if (collision.gameObject.name == "EndNote")
            {
                Debug.Log("enemy end long note");
                Note note = collision.GetComponent<Note>();
                note.enabled = false;

                GameObject parentGameObject = collision.transform.parent?.gameObject;

                if (parentGameObject != null)
                {
                    parentGameObject.transform.position = tfEnemyLongNoteAppear.position;
                    parentGameObject.gameObject.SetActive(false);
                    ObjectPool.instance.enemyLongNoteQueue.Enqueue(parentGameObject.gameObject);
                }
            }
        }
        else
        {
            if (collision.CompareTag("Note"))
            {
                ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
                collision.gameObject.SetActive(false);

            }
        }
    }
    public void MakeNote()
    {
        GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
        t_note.transform.position = tfEnemyNoteAppear.position;
        Note note = t_note.GetComponent<Note>();
        
        t_note.SetActive(true);
        note.noteDir = Vector3.left;
    }

    public void MakeLongNote()
    {
        GameObject t_note = ObjectPool.instance.enemyLongNoteQueue.Dequeue();
        RectTransform rect = t_note.GetComponent<RectTransform>();
        longnote3 longnote3 = t_note.GetComponent<longnote3>();
        
        rect.position = tfEnemyLongNoteAppear.position;

        longnote3.isNoteActive = true;
        longnote3.startNote.transform.position = tfEnemyNoteAppear.position;
        longnote3.endNote.transform.position = tfEnemyNoteAppear.position;

        t_note.SetActive(true);
        longnote3.startNote.SetActive(true);
        longnote3.endNote.SetActive(true);
        longnote3.lineObject.SetActive(true);
        Note startNote = longnote3.startNote.GetComponent<Note>();
        Note endNote = longnote3.endNote.GetComponent<Note>();
        endNote.enabled = true;
        startNote.noteDir = Vector3.left;
        endNote.noteDir = Vector3.left;
        longnote3.FinishEndNote();
    }
}
