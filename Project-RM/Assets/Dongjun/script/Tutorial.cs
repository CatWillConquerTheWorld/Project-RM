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
        yield return StartCoroutine(chatting.Chat(4f, 0.75f, "자네가 다음 지원자인가?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5f, 1.5f, "이렇게 건강한 사람들만 와서야 원..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "아, 자네 앞에 셀 수도 없이 많은 지원자가 있었다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "다 어떻게 됐냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "자네 순서까지 왔으면 알 법도 하지 않나?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "잡소리는 여기까지 하고,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "이왕 왔으니 내 친절히 알려드리리다."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "자네는 지금부터 '부패한 성' 으로 들어간다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "그 안에는 썩은 고위 전사들과 마법사들,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "그리고 괴생명체들이 있다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "자네 임무는 부패한 왕을 찾아 없애는 것일세."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "어떻게 없애냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "아무래도 왕의 숨통을 끊어야겠지."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "맨몸으로 가냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "말 한번 잘했네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "장비는 줄걸세. 그리고 사용법도 친히 알려주지."));
        //카메라 무빙
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

}
