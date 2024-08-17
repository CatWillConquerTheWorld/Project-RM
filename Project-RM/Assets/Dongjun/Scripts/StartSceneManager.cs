using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public PlayerController playerPlayerController;
    public GameObject playerGun;

    public CanvasGroup startCanvasGroup;
    public TMP_Text pressStart;

    private bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        isStarted = false;
        //PlayerPrefs.SetInt("tutorialCleared", 1);
        playerGun.SetActive(false);
        playerPlayerController.enabled = false;
        pressStart.DOFade(0.1f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isStarted)
        {
            StartCoroutine(StartFlow());
        }
    }

    IEnumerator StartFlow()
    {
        isStarted = true;
        pressStart.DOKill();
        startCanvasGroup.DOFade(0f, 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1);
        if (PlayerPrefs.GetInt("tutorialCleared") == 0)
        {
            StartCoroutine(Tutorial.Instance.TutorialFlow());
        }
        else
        {
            StartCoroutine(Tutorial.Instance.MovieEnd());
        }
        yield return null;
    }
}
