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
        if(Input.GetKeyDown(KeyCode.A))
        {
            timingManager.CheckTiming();
        }
    }
}
