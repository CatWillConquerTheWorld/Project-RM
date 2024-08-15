using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;
public class longnote3 : MonoBehaviour
{
    public GameObject startNote, endNote;
    public RectTransform startUI;
    public RectTransform endUI; // �� UI ������Ʈ
    private RectTransform line;
    public GameObject lineObject;
    public Image lineImg;
    public RectTransform lineRect;
    public Note endNoteScript;
    public int bpm;
    public float bpminterval;
    double currentTime = 0d;
    public bool isNoteActive = true;

    RectTransform noteSpawnPos;

    void Start()
    {
        // lineImg.color = Color.black; // ���� ���� ����
        startNote.SetActive(true);
        endNote.SetActive(true);
        lineObject.SetActive(true);
        lineRect.SetParent(transform, false); // �θ� ����
        // lineRect.localPosition = Vector3.zero; // ���� ��ġ�� (0, 0)���� �ʱ�ȭ
        noteSpawnPos = startNote.GetComponent<RectTransform>();
        bpm = 120;
        bpminterval = 60f / bpm;
    }

    private void OnEnable()
    {
        currentTime = 0d;
        FinishEndNote();
    }

    // Update is called once per frame
    void Update()
    {
        Resize();
        if (startNote.activeSelf == false && endNote.activeSelf == false)
        {
            lineObject.SetActive(false);
            gameObject.SetActive(false);
        }

        if (isNoteActive)
        {
            currentTime += Time.deltaTime;
            
            if (currentTime >= bpminterval * 2) // 1��Ʈ�� �ð� (2��)
            {
                StartEndNote();
                isNoteActive = false;
            }
        }
    }


    public void Resize()
    {
        Vector3 startPos = startUI.position;
        Vector3 endPos = endUI.position;

        float distance = Vector3.Distance(startPos, endPos);
        lineRect.sizeDelta = new Vector2(distance, 80f); // ���� ���̿� �β� ����

        // ���� �߾� ��ġ ����
        lineRect.position = (startPos + endPos) / 2;

        // ���� ȸ�� ���� (���� ��������)
        float angle = Vector3.Angle(Vector3.right, endPos - startPos);
        if (endPos.y < startPos.y) angle = -angle; // ���⿡ ���� ���� ����
        lineRect.rotation = Quaternion.Euler(0, 0, angle); // ���� ����
    }


    public void StartEndNote()
    {
        Note note = endNote.GetComponent<Note>();
        note.enabled = true;
    }

    public void FinishEndNote()
    {
        if (endNoteScript == null)
        {
            endNoteScript = endNote.GetComponent<Note>();
        }
        endNoteScript.enabled = false;
    }
}
