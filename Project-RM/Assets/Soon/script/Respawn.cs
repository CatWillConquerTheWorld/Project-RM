using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update

    public Image fadeImage;
    public float fadeSpeed = 1.0f;
    public bool isFadeOut = false;
    private void Start()
    {
        //StartCoroutine(FadeIn());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        if (isFadeOut)
            yield break;

        isFadeOut = true;

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.5f);

            SceneManager.LoadScene("scene3_soon");
        }
    }
}
