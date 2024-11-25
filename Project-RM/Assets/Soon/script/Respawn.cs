using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject platform;

    public RectTransform greyBG_up;
    public RectTransform greyBG_down;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {         
            CenterFrame.MusicFadeOut();
            greyBG_up.DOAnchorPosY(270f, 0.3f).SetEase(Ease.InSine);
            greyBG_down.DOAnchorPosY(-270f, 0.3f).SetEase(Ease.InSine);
            platform.SetActive(false);
            StartCoroutine(Restart());
        }
    }
    IEnumerator Restart()
    {     
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("scene3_soon");
    }

}
