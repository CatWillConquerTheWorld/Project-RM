
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;

    UnityEngine.UI.Image noteImage;

    public bool isEnemyNote = false;
    public Vector3 noteDir = Vector3.right;

    void OnEnable()
    {
        if(noteImage == null)
        {
            noteImage = GetComponent<UnityEngine.UI.Image>();
        }
        
        noteImage.enabled = true;

        // noteDir = Vector3.right;
    }
    // Update is called once per frame
    void Update()
    {
       moveNote();
    }

    public void moveNote()
    {
        transform.position += noteDir * noteSpeed * Time.deltaTime;
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
