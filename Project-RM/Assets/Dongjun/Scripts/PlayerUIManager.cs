using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    public Image healthBar;

    public Image lowHealthIndicator;

    private float amount;

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
        if (healthBar.fillAmount < 0.4f) amount = 0.1f;
        if (healthBar.fillAmount < 0.3f) amount = 0.2f;
        if (healthBar.fillAmount < 0.15f) amount = 0.4f;
        if (healthBar.fillAmount >= 0.4f) amount = 0f;
        lowHealthAnim();
    }

    public void lowHealthAnim()
    {
        lowHealthIndicator.DOFade(amount, 0.5f).SetEase(Ease.InOutSine);
    }
}
