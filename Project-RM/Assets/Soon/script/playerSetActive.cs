using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSetActive : MonoBehaviour
{
    public GameObject player;

    public void playerDisappear()
    {
        player.SetActive(false);
    }

    public void playerAappear()
    {
        player.SetActive(true);
    }
}
