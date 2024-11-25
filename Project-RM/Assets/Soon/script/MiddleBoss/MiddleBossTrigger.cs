using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBossTrigger : MonoBehaviour
{
    public MiddleBossManager mbm;
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(mbm.MiddleBossEnter());
        }
    }
}
