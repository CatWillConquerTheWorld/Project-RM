using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackBeam : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void disableBoxCollider()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }
    
    void killSelf()
    {
        Destroy(this.gameObject);
    }
}
