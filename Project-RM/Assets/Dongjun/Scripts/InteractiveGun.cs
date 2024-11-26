using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGun : MonoBehaviour
{
    public GameObject playerGun;

    public GameObject keyF;
    public Material outline;
    public Material noOutline;
    private Material myMaterial;

    private bool isPlayerEnteredFieldOnce;
    private bool isPlayerHoldingGun;

    void Start()
    {
        isPlayerEnteredFieldOnce = false;
        isPlayerHoldingGun = false;
        myMaterial = GetComponent<Material>();
        keyF.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            keyF.SetActive(true);
            GetComponent<SpriteRenderer>().material = outline;
            if (!isPlayerEnteredFieldOnce)
            {
                Tutorial.Instance.canHoldGun = true;
                isPlayerEnteredFieldOnce = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPlayerHoldingGun)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Tutorial.Instance.holdingGun = true;
                isPlayerHoldingGun = true;
                playerGun.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            keyF.SetActive(false);
            GetComponent<SpriteRenderer>().material = noOutline;
        }
    }
}
