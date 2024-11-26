using UnityEngine;

public class playerController : MonoBehaviour
{
    public timingManager timingManager;

    private void Start()
    {
        //timingManager = FindObjectOfType<timingManager>();
    }
    void Update()
    {
        int size = timingManager.boxNoteList.Count;
        if (Input.GetKeyDown(KeyCode.A) && size > 0)
        {
            timingManager.CheckTiming();
        }
    }
}
