using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    AudioSource myAudio;
    public bool musicStart = false;
    
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                myAudio.Play();
                musicStart = true;
            }
        }
        
    }

    public void MusicStart()
    {
        myAudio?.Play();
        musicStart = true;
    }
}
