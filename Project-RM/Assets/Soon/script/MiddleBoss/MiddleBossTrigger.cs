using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class MiddleBossTrigger : MonoBehaviour
{
    public MiddleBossManager mbm;
    public BoxCollider2D bc;
    // Start is called before the first frame update

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(mbm.MiddleBossEnter()); 
            bc.enabled = false;
        }
    }
}
