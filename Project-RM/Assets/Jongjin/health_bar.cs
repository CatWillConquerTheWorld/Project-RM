using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class health_bar : MonoBehaviour
{
    private PlayerController playerController; // PlayerController 스크립트 참조
    private UnityEngine.UI.Image healthBarBackgroundImage; // 체력바 뒷배경 이미지 UI 요소
    private UnityEngine.UI.Image healthBarImage; // 체력바 이미지 UI 요소
    
    private TextMeshProUGUI Health;
    private float maxHealth;
    private float currentHealth;

    private void Start()
    {
        healthBarImage = GameObject.Find("HealthBar").GetComponent<UnityEngine.UI.Image>();
        Health = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();
        
        SetPlayerHp();
    }
    private void Update(){
        healthBarImage.fillAmount = currentHealth/maxHealth;
        Health.text = currentHealth + "/" + maxHealth;
        if(Input.GetKeyDown(KeyCode.A)){
            currentHealth -= 10;
            Debug.Log("1");
        }
        
    }
    void SetPlayerHp(){
        maxHealth = 1000;
        currentHealth = 1000;
    }
    
}
