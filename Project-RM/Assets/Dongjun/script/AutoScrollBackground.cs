using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AutoScrollBackground : MonoBehaviour
{
    //public GameObject container;

    public GameObject targetPos;
    public GameObject returnPos;
    public float moveSpeed;

    void Update()
    {
        if (transform.position.x <= targetPos.transform.position.x)
        {
            transform.position = returnPos.transform.position;
        } else
        {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
