using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollBackground : MonoBehaviour
{
    public float targetPos;
    public float returnPos;
    public float moveSpeed;

    void Update()
    {
        
        if (transform.position.x > targetPos)
        {
            print("Done");
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y , transform.position.z);
        } else
        {
            transform.position = new Vector3(returnPos, transform.position.y, transform.position.z);
        }
    }
}
