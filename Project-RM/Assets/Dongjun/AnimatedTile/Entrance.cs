using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public Animator animator;
    public GameObject keyF;

    void Start()
    {
        keyF.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            animator.SetBool("lightUp", true);
            keyF.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                SceneLoader.LoadSceneWithLoading("Stage1");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("lightUp", false);
            keyF.SetActive(false);
        }
    }
}
