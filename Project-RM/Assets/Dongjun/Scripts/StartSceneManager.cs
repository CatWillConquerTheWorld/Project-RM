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
        Pause.isOKToPause = false;
        isStarted = false;
        PlayerPrefs.SetInt("tutorialCleared", 0);
        playerGun.SetActive(false);
        playerPlayerController.enabled = false;
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
        startCanvasGroup.DOFade(0f, 1f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(1);
        if (PlayerPrefs.GetInt("tutorialCleared") == 0)
        {
            StartCoroutine(Tutorial.Instance.TutorialFlow());
        }
        else
        {
            StartCoroutine(Tutorial.Instance.MovieEnd());
            playerPlayerController.enabled = true;
            Pause.isOKToPause = true;
        }
        yield return null;
    }
}
