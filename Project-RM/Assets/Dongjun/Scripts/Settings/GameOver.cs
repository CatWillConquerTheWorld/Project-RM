using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;

    public Image bg;
    public Image gameOver;
    public GameObject buttonContainer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        bg.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        bg.DOFade(0f, 0.00001f);
        gameOver.DOFade(0f, 0.00001f);
        buttonContainer.SetActive(false);
    }

    public IEnumerator GameOverAnim()
    {
        bg.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(true);
        bg.DOFade(1f, 1f).SetEase(Ease.OutSine);
        gameOver.DOFade(1f, 1f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1.5f);
        buttonContainer.SetActive(true);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
