using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public Animator animator;


    void Start()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") animator.SetBool("lightUp", true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") animator.SetBool("lightUp", false);
    }
}
