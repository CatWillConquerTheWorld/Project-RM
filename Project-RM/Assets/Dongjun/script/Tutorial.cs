using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject chatManager;

    private Chatting chatting;

    private bool isNext;
    
    void Start()
    {
        chatting = chatManager.GetComponent<Chatting>();
        StartCoroutine(TutorialFlow());

        isNext = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            isNext = true;
        }
    }

    IEnumerator TutorialFlow()
    {
        yield return StartCoroutine(chatting.Chat(4f, 1.5f, "상처를 치료해줄 사람 어디 없나"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5f, 1.5f, "가만히 놔뒀다간 끊임없이 덧나"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "사랑도 사람도 너무나도 겁나"));
        //yield return new WaitForSeconds(1f);
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

}
