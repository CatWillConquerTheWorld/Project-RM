using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

        chatDelay = new WaitForSeconds(0.05f);
    }

    void Update()
    {
    }


    public IEnumerator Chat(float chatSizeX, float chatSizeY, string text)
    {
        innerText.GetComponent<RectTransform>().sizeDelta = new Vector2(chatSizeX - 1, chatSizeY);
        innerTextTMPro.text = "";
        chatterSpriteRenderer.size = new Vector2 (chatSizeX, chatSizeY);

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != ' ')
            {
                audioSource.Play();
            }
            innerTextTMPro.text += text[i];
            yield return chatDelay;
        }
    }
}
