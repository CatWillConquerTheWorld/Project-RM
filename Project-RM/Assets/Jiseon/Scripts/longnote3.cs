
using UnityEngine;
using UnityEngine.UI;
public class longnote3 : MonoBehaviour
{
    public GameObject startNote, endNote;
    public RectTransform startUI;
    public RectTransform endUI; // 끝 UI 오브젝트
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
        // lineImg.color = Color.black; // 선의 색상 설정
        startNote.SetActive(true);
        endNote.SetActive(true);
        lineObject.SetActive(true);
        lineRect.SetParent(transform, false); // 부모 설정
        // lineRect.localPosition = Vector3.zero; // 로컬 위치를 (0, 0)으로 초기화
        noteSpawnPos = startNote.GetComponent<RectTransform>();
        bpm = bpmManager.instance.bpm;
        bpminterval = bpmManager.instance.bpmInterval;
    }

    private void OnEnable()
    {
        currentTime = 0d;

        // lineObject의 위치와 크기를 초기화
        Resize();

        // lineObject를 활성화하기 전에 미리 위치를 설정
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
            
        //    if (currentTime >= bpminterval * 2) // 1비트의 시간 (2박)
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
        lineRect.sizeDelta = new Vector2(distance, 80f); // 선의 길이와 두께 설정

        // 선의 중앙 위치 설정
        lineRect.position = (startPos + endPos) / 2;

        // 선의 회전 설정 (세로 방향으로)
        float angle = Vector3.Angle(Vector3.right, endPos - startPos);
        if (endPos.y < startPos.y) angle = -angle; // 방향에 따라 각도 조정
        lineRect.rotation = Quaternion.Euler(0, 0, angle); // 각도 설정


        //// 각 노트의 RectTransform에서 왼쪽 및 오른쪽 끝 위치 계산
        //Vector3 startLeft = startUI.TransformPoint(new Vector3(startUI.rect.xMin + 3f, startUI.rect.center.y, 0));
        //Vector3 endRight = endUI.TransformPoint(new Vector3(endUI.rect.xMax - 3f, endUI.rect.center.y, 0));

        //// 라인의 길이를 시작 노트와 끝 노트 사이의 거리로 설정
        //float distance = Vector3.Distance(startLeft, endRight);
        //lineRect.sizeDelta = new Vector2(distance, 80f); // 선의 길이와 두께 설정

        //// 라인의 중앙 위치를 시작 노트의 왼쪽 끝과 끝 노트의 오른쪽 끝의 중간 지점으로 설정
        //lineRect.position = (startLeft + endRight) / 2;

        //// 선의 회전 설정 (세로 방향으로)
        //float angle = Vector3.Angle(Vector3.right, endRight - startLeft);
        //if (endRight.y < startLeft.y) angle = -angle; // 방향에 따라 각도 조정
        //lineRect.rotation = Quaternion.Euler(0, 0, angle); // 각도 설정
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
