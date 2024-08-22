
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
    public double bpminterval;
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
        bpm = bpmManager.instance.bpm;
        bpminterval = bpmManager.instance.bpmInterval;
    }

    private void OnEnable()
    {
        currentTime = 0d;

        // lineObject�� ��ġ�� ũ�⸦ �ʱ�ȭ
        Resize();

        // lineObject�� Ȱ��ȭ�ϱ� ���� �̸� ��ġ�� ����
        lineRect.position = (startUI.position + endUI.position) / 2;


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
            ObjectPool.instance.longNoteQueue.Enqueue(gameObject);
        }

        //if (isNoteActive)
        //{
        //    currentTime += Time.deltaTime;
            
        //    if (currentTime >= bpminterval * 2) // 1��Ʈ�� �ð� (2��)
        //    {
        //        StartEndNote();
        //        isNoteActive = false;
        //    }
        //}
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


        //// �� ��Ʈ�� RectTransform���� ���� �� ������ �� ��ġ ���
        //Vector3 startLeft = startUI.TransformPoint(new Vector3(startUI.rect.xMin + 3f, startUI.rect.center.y, 0));
        //Vector3 endRight = endUI.TransformPoint(new Vector3(endUI.rect.xMax - 3f, endUI.rect.center.y, 0));

        //// ������ ���̸� ���� ��Ʈ�� �� ��Ʈ ������ �Ÿ��� ����
        //float distance = Vector3.Distance(startLeft, endRight);
        //lineRect.sizeDelta = new Vector2(distance, 80f); // ���� ���̿� �β� ����

        //// ������ �߾� ��ġ�� ���� ��Ʈ�� ���� ���� �� ��Ʈ�� ������ ���� �߰� �������� ����
        //lineRect.position = (startLeft + endRight) / 2;

        //// ���� ȸ�� ���� (���� ��������)
        //float angle = Vector3.Angle(Vector3.right, endRight - startLeft);
        //if (endRight.y < startLeft.y) angle = -angle; // ���⿡ ���� ���� ����
        //lineRect.rotation = Quaternion.Euler(0, 0, angle); // ���� ����
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
