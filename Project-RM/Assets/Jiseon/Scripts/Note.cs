
using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 1800;

    UnityEngine.UI.Image noteImage;

    public bool isEnemyNote = false;
    public Vector3 noteDir = Vector3.right;

    public RectTransform startPoint; // 시작 위치 오브젝트
    public RectTransform targetPoint; // 판정기 오브젝트
    private float Speed; // 계산된 노트 속도


    void OnEnable()
    {
        if(noteImage == null)
        {
            noteImage = GetComponent<UnityEngine.UI.Image>();
        }
        startPoint = GameObject.Find("NoteAppearLocation").GetComponent<RectTransform>();
        targetPoint = GameObject.Find("CenterFrame").GetComponent <RectTransform>();
        noteImage.enabled = true;
        float distanceToTarget = Vector3.Distance(startPoint.position, targetPoint.position);
        float sixteenthNoteTime = 60f / bpmManager.instance.bpm; // 16분음표 시간
        Speed = distanceToTarget / sixteenthNoteTime; // 이동 속도 계산
        // noteDir = Vector3.right;
    }
    // Update is called once per frame
    void Update()
    {
       moveNote();
    }

    public void moveNote()
    {
        transform.position += noteDir * Speed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled; // trie면 이미지가 보여지고 있는상태
    }
}
