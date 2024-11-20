using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    public Image healthBar;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ManageHealth(float maxHealth, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        //print(currentHealth / maxHealth);
    }
}
