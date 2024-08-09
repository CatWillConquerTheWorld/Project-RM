using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public GameObject chatManager;
    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public GameObject infoText;

    public GameObject gun;

    private RectTransform infoTextRect;
    private TextMeshProUGUI infoTextTMP;

    private Chatting chatting;

    //skipping to next description
    private bool isNext;

    //checks if player is in the interactive gun's field
    public bool canHoldGun;
    public bool holdingGun;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        chatting = chatManager.GetComponent<Chatting>();

        infoTextRect = infoText.GetComponent<RectTransform>();
        infoTextTMP = infoText.GetComponent<TextMeshProUGUI>();

        canHoldGun = false;
        holdingGun = false;

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
        yield return StartCoroutine(WaitForUser());
        //ī�޶� ����
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "�� �տ� ���� ���̴°�? ���� �ѹ� ��ƺ���."));
        yield return StartCoroutine(MovieEnd());
        //ī�޶� ���� ����
        InfoTextChange("����Ű�� ���� �̵��ϼ���.");
        InfoTextAppear();
        yield return StartCoroutine(WaitForGun());
        InfoTextDisappear();
        StartCoroutine(MovieStart());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "������� �����ӽ��ϴ�! ���߿� �����ϼ���!"));
        //ī�޶� ����
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

    IEnumerator MovieStart()
    {
        movieEffectorUp.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutSine);
        movieEffectorDown.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutSine);
        yield return null;
    }

    IEnumerator MovieEnd()
    {
        movieEffectorUp.DOAnchorPosY(150f, 0.5f).SetEase(Ease.OutSine);
        movieEffectorDown.DOAnchorPosY(-150f, 0.5f).SetEase(Ease.OutSine);
        yield return null;
    }

    IEnumerator WaitForGun()
    {
        canHoldGun = false;
        while (!canHoldGun) yield return null;
        InfoTextChange("F�� ���� ���� ��������.");
        while (!holdingGun) yield return null;
        gun.SetActive(false);
        yield return null;
    }

    void InfoTextAppear()
    {
        infoTextRect.DOAnchorPosY(-50f, 0.5f).SetEase(Ease.OutSine);
    }

    void InfoTextDisappear()
    {
        infoTextRect.DOAnchorPosY(50f, 0.5f).SetEase(Ease.OutSine);
    }

    void InfoTextChange(string text)
    {
        infoTextTMP.text = text;
    }
}
