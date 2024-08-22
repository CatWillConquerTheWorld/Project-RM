
using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 1800;

    UnityEngine.UI.Image noteImage;

    public bool isEnemyNote = false;
    public Vector3 noteDir = Vector3.right;

    public RectTransform startPoint; // ���� ��ġ ������Ʈ
    public RectTransform targetPoint; // ������ ������Ʈ
    private float Speed; // ���� ��Ʈ �ӵ�


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
        float sixteenthNoteTime = 60f / bpmManager.instance.bpm; // 16����ǥ �ð�
        Speed = distanceToTarget / sixteenthNoteTime; // �̵� �ӵ� ���
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
        return noteImage.enabled; // trie�� �̹����� �������� �ִ»���
    }
}
