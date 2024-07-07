using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void enableBoxCollider()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;
    }

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
