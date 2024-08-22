using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    public int bpm; // 분당 비트 수 의미
    double currentTime = 0d;

    [SerializeField] Transform tfEnemyNoteAppear = null; // 노트 생성위치
    [SerializeField] RectTransform tfEnemyLongNoteAppear = null; // 노트 생성위치

    bool notecheck = false; // long short 번갈아가며 출력위해.
    
    public static bool isMusicStart;

    void Start()
    {
        isMusicStart = false;
        bpm = bpmManager.instance.bpm;
    }

    void Update()
    {
        // if (isMusicStart) NoteMaking();
        if (!isMusicStart) currentTime = 0f;
    }

    GameObject curLongNote;
    longnote3 curLongNoteScript;
    bool is_longnote_start = false;

    public void NoteMaker(string note)
    {
        if (note == "AA")
        {
            MakeNote();

        }
        else if (note == "AB")
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
        }
        else if (note == "00")
        {

        }
    }


    public void NoteMaking()
    {
        currentTime += Time.deltaTime;
        double BeatTime = bpmManager.instance.bpmInterval * 3;
        if (currentTime >= BeatTime && gameObject.activeSelf) // 1비트의 시간 
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
                List<GameObject> enemies = Stage1.enemies;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].name == "Assasin(Clone)") enemies[i].GetComponent<Assasin>().LongAttack();
                    else if (enemies[i].name == "OrbMage(Clone)") enemies[i].GetComponent<OrbMage>().LongAttack();
                    else if (enemies[i].name == "Enemy_Archer(Clone)") enemies[i].GetComponent<Archer>().LongAttack();
                    else if (enemies[i].name == "Enemy_Sweeper(Clone)") enemies[i].GetComponent<Sweeper>().LongAttack();
                }
            }
            else
            {
                List<GameObject> enemies = Stage1.enemies;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].name == "Assasin(Clone)") enemies[i].GetComponent<Assasin>().LongAttackPrepare();
                    else if (enemies[i].name == "OrbMage(Clone)") enemies[i].GetComponent<OrbMage>().LongAttackPrepare();
                    else if (enemies[i].name == "Enemy_Archer(Clone)") enemies[i].GetComponent<Archer>().LongAttackPrepare();
                    else if (enemies[i].name == "Enemy_Sweeper(Clone)") enemies[i].GetComponent<Sweeper>().LongAttackPrepare();
                }
            }
        }
        else
        {
            if (collision.CompareTag("Note"))
            {
                ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
                collision.gameObject.SetActive(false);
                List<GameObject> enemies = Stage1.enemies;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].name == "Assasin(Clone)") enemies[i].GetComponent<Assasin>().Attack();
                    else if (enemies[i].name == "OrbMage(Clone)") enemies[i].GetComponent<OrbMage>().Attack();
                    else if (enemies[i].name == "Enemy_Archer(Clone)") enemies[i].GetComponent<Archer>().Attack();
                    else if (enemies[i].name == "Enemy_Sweeper(Clone)") enemies[i].GetComponent<Sweeper>().Attack();
                    else if (enemies[i].name == "Enemy_CagedShocker(Clone)") enemies[i].GetComponent<CagedShocker>().Attack();
                    else if (enemies[i].name == "Enemy_BombDroid(Clone)") enemies[i].GetComponent<BombDroid>().Attack();
                    else if (enemies[i].name == "Enemy_Warden(Clone)") enemies[i].GetComponent<Warden>().Attack();
                }
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
        curLongNote = t_note;
        curLongNoteScript = curLongNote.GetComponent<longnote3>();
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
