using UnityEngine;

public class playerController : MonoBehaviour
{
    timingManager timingManager;

    private void Start()
    {
        timingManager = FindObjectOfType<timingManager>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            timingManager.CheckTiming();
        }
    }
}
