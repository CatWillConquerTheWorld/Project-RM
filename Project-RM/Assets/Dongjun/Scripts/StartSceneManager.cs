using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public PlayerController playerPlayerController;
    public GameObject playerGun;

    public CanvasGroup startCanvasGroup;
    public RectTransform top;
    public RectTransform bottom;
    public TMP_Text pressStart;

    private bool isStarted;
    private bool startAnimationFinished;

    public GameObject pauseButton;

    // Start is called before the first frame update
    void Start()
    {
        pauseButton.SetActive(false);
        startCanvasGroup.alpha = 0f;
        startCanvasGroup.DOFade(1f, 2f).SetEase(Ease.InOutSine);
        top.DOScaleY(0.125f, 1f).SetEase(Ease.OutSine).SetDelay(2f);
        bottom.DOScaleY(0.125f, 1f).SetEase(Ease.OutSine).SetDelay(2f).OnComplete(() => startAnimationFinished = true);
        Pause.isOKToPause = false;
        isStarted = false;
        playerGun.SetActive(false);
        playerPlayerController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isStarted && startAnimationFinished)
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
            pauseButton.SetActive(true);
        }
        top.gameObject.SetActive(false);
        bottom.gameObject.SetActive(false);
        yield return null;
    }
}
