using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0; // 분당 비트 수 의미
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear = null; // 노트 생성위치
    // [SerializeField] GameObject goNote = null; // 노트 프리팹

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

        if (currentTime >= 60d / bpm) // 1비트의 시간 
        {
            GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
            t_note.transform.position = tfNoteAppear.position;
            t_note.SetActive(true);
            timingManager.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
