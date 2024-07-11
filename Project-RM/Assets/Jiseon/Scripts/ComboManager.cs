using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage = null;

    public int currentCombo = 0;

    Animator myAnim;
    string animComboUp = "ComboUp";

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        goComboImage.SetActive(false);
    }
    public void IncreaseCombo(int n_num = 1) // 넘기는 값 없으면 기본 1
    {
        currentCombo += n_num;
        Debug.Log("Combo = " + currentCombo);
        if(currentCombo >= 10)
        {
            goComboImage.SetActive(true);
            myAnim.SetTrigger(animComboUp);
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        Debug.Log("Reset Combo = " + currentCombo);
        goComboImage.SetActive(false);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public bool GetCurrentComboBool()
    {
        return currentCombo >= 10;
    }
}
