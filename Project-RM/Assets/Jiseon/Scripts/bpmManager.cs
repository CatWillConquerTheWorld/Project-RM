using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bpmManager : MonoBehaviour
{
    public static bpmManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance!= this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public int bpm = 120;
    public double bpmInterval ;

    void Start()
    {
        bpmInterval = 60d / bpm;
    }

    private void Update()
    {
        bpmInterval = 60d / bpm;
    }

}
