using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chatting : MonoBehaviour
{
    public GameObject chatter;
    public GameObject innerText;
    public AudioSource audioSource;


    private SpriteRenderer chatterSpriteRenderer;
    private TextMeshPro innerTextTMPro;

    private WaitForSeconds chatDelay;


    void Start()
    {
        //chatter.SetActive(false);
        //innerText.SetActive(false);
        chatterSpriteRenderer = chatter.GetComponent<SpriteRenderer>();
        innerTextTMPro = innerText.GetComponent<TextMeshPro>();

        chatDelay = new WaitForSeconds(0.03f);
    }

    void Update()
    {
    }


    public IEnumerator Chat(float chatSizeX, string text)
    {
        innerText.GetComponent<RectTransform>().sizeDelta = new Vector2(chatSizeX - 1f, 0.75f);
        innerTextTMPro.text = "";
        chatterSpriteRenderer.size = new Vector2 (chatSizeX, 0.75f);

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != ' ')
            {
                audioSource.Play();
            }
            innerTextTMPro.text += text[i];
            yield return chatDelay;
        }
        yield return chatDelay;
    }

    public void EnableChat()
    {
        chatter.SetActive(true);
    }

    public void DisableChat()
    {
        chatter.SetActive(false);
    }
}
