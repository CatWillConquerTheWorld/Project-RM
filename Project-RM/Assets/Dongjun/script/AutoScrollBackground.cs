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
        ////print(name + transform.position.x);
        //if (transform.position.x > targetPos.transform.position.x + container.transform.position.x)
        //{
        //    //print("Done");
        //    transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y , transform.position.z);
        //} else
        //{
        //    transform.position = new Vector3(returnPos + container.transform.position.x, transform.position.y, transform.position.z);
        //}

        if (transform.position.x <= targetPos.transform.position.x)
        {
            transform.position = returnPos.transform.position;
        } else
        {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
