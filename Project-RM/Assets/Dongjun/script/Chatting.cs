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
        chatter.SetActive(false);
        innerText.SetActive(false);
        chatterSpriteRenderer = chatter.GetComponent<SpriteRenderer>();
        innerTextTMPro = innerText.GetComponent<TextMeshPro>();

        chatDelay = new WaitForSeconds(0.075f);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            chatter.SetActive(true);
            innerText.SetActive(true);
            StartCoroutine(Chat(4, 0.75f, "¿ï¶ö¶ó ¼¼¼Ç"));
        }
    }


    IEnumerator Chat(float chatSizeX, float chatSizeY, string text)
    {
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
