using UnityEngine;

public class PBGforStage: MonoBehaviour
{
    private float startingPos;
    public float AmountOfParallax;
    public float AmountOfParallaxY;
    public Camera MainCamera;

    void Start()
    {
        startingPos = transform.position.x;
    }

    void Update()
    {
        DoParallax();
    }

    void DoParallax()
    {
        Vector3 pos = MainCamera.transform.position;
        float dist = pos.x * AmountOfParallax;
        float distY = pos.y * AmountOfParallaxY;

        Vector3 newPos = new Vector3(startingPos + dist, startingPos + distY, transform.position.z);

        transform.position = newPos;
    }
}
