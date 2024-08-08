using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private float startingPos;
    public float AmountOfParallax;
    public Camera MainCamera;

    void Start()
    {
        startingPos = transform.position.x;
    }

    void Update()
    {
        Vector3 pos = MainCamera.transform.position;
        float temp = pos.x * (1 - AmountOfParallax);
        float dist = pos.x * AmountOfParallax;

        Vector3 newPos = new Vector3(startingPos + dist, transform.position.y, transform.position.z);

        transform.position = newPos;
    }
}
