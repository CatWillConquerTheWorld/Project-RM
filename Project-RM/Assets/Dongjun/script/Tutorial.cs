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
        yield return StartCoroutine(chatting.Chat(4f, 1.5f, "��ó�� ġ������ ��� ��� ����"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5f, 1.5f, "������ ���״ٰ� ���Ӿ��� ����"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "����� ����� �ʹ����� �̳�"));
        //yield return new WaitForSeconds(1f);
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

}
