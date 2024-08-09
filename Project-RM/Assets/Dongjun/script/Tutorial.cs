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
        yield return StartCoroutine(chatting.Chat(4f, 0.75f, "�ڳװ� ���� �������ΰ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5f, 1.5f, "�̷��� �ǰ��� ����鸸 �ͼ��� ��..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "��, �ڳ� �տ� �� ���� ���� ���� �����ڰ� �־��ٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�� ��� �Ƴİ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�ڳ� �������� ������ �� ���� ���� �ʳ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "��Ҹ��� ������� �ϰ�,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�̿� ������ �� ģ���� �˷��帮����."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�ڳ״� ���ݺ��� '������ ��' ���� ���ٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�� �ȿ��� ���� ���� ������ �������,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�׸��� ������ü���� �ִٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�ڳ� �ӹ��� ������ ���� ã�� ���ִ� ���ϼ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "��� ���ֳİ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�ƹ����� ���� ������ ����߰���."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�Ǹ����� ���İ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�� �ѹ� ���߳�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "���� �ٰɼ�. �׸��� ������ ģ�� �˷�����."));
        //ī�޶� ����
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

}
