using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGun : MonoBehaviour
{
    public Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void back()
    {
        myAnimator.SetTrigger("back");
    }
}
