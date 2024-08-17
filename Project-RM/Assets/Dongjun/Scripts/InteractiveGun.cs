using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGun : MonoBehaviour
{
    public GameObject playerGun;

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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
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
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<SpriteRenderer>().material = noOutline;
        }
    }
}
