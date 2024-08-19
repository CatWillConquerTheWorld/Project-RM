using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TestRoomDoor : MonoBehaviour
{
    public GameObject KeyF;
    public static GameObject DoorLeft;
    public static GameObject DoorRight;
    public Material outline;
    public Material noOutline;
    public TMP_Text startBattle;

    // Start is called before the first frame update
    void Start()
    {
        DoorLeft = GameObject.Find("Door");
        DoorRight = GameObject.Find("Door (1)");
        KeyF.SetActive(false);
        startBattle.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            KeyF.SetActive(true);
            startBattle.enabled = true;
            GetComponent<SpriteRenderer>().material = outline;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Tutorial.Instance.isDoorOpened = true;
                KeyF.SetActive(false);
                startBattle.enabled = false;
                GetComponent<SpriteRenderer>().material = noOutline;
                DoorLeft.GetComponent<Animator>().SetTrigger("DoorOpen");
                DoorRight.GetComponent<Animator>().SetTrigger("DoorOpen");
                DoorLeft.GetComponent<BoxCollider2D>().enabled = false;
                DoorLeft.transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = false;
                DoorRight.GetComponent<BoxCollider2D>().enabled = false;
                DoorRight.transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            KeyF.SetActive(false);
            startBattle.enabled = false;
            GetComponent<SpriteRenderer>().material = noOutline;
        }
    }

    public static void DoorClose()
    {
        DoorLeft.GetComponent<Animator>().SetTrigger("DoorClose");
        DoorRight.GetComponent<Animator>().SetTrigger("DoorClose");
    }

    public static void Reset()
    {
        DoorLeft.GetComponent<BoxCollider2D>().enabled = true;
        DoorLeft.transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = true;
        DoorRight.GetComponent<BoxCollider2D>().enabled = true;
        DoorRight.transform.Find("Trigger").GetComponent<BoxCollider2D>().enabled = true;
        DoorClose();
    }
}
