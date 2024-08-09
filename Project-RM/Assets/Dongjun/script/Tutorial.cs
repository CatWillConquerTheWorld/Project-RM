using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public Camera mainCamera;
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
        yield return StartCoroutine(chatting.Chat(4.1f, "�ڳװ� ���� �������ΰ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.7f, "�̷��� �ǰ��� ����鸸 �ͼ��� ��..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6f,"��, �ڳ� �տ� ���� �����ڰ� �־��ٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.15f, "�� ��� �Ƴİ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.3f, "�ڳ� �������� ������ �� ���� ���� �ʳ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4f, "��Ҹ��� ������� �ϰ�,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.65f, "�̿� ������ �� ģ���� �˷��帮����."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.9f,"�ڳ״� ���ݺ��� '������ ��' ���� ���ٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.1f, "�� �ȿ��� ���� ���� ������ �������,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.6f, "�׸��� ������ü���� �ִٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.8f, "�ڳ� �ӹ��� ������ ���� ã�� ���ִ� ���ϼ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.1f, "��� ���ֳİ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.3f, "�ƹ����� ���� ������ ����߰���."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.05f, "�Ǹ����� ���İ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.83f, "�� �ѹ� ���߳�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(7.1f, "���� �ٰɼ�. �׸��� ������ ģ�� �˷�����."));
        yield return StartCoroutine(WaitForUser());
        //ī�޶� ����
        yield return StartCoroutine(CameraMoveX(10f, 1f, "flex"));
        yield return StartCoroutine(chatting.Chat(6.7f, "�� �տ� ���� ���̴°�? ���� �ѹ� ��ƺ���."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(MovieEnd());
        //ī�޶� ���� ����
        yield return StartCoroutine(CameraMoveX(-10f, 1f, "flex"));
        //yield return StartCoroutine(CameraMoveX(-20f, 0.5f));
        CameraReturns();
        InfoTextChange("����Ű�� ���� �̵��ϼ���.");
        InfoTextAppear();
        yield return StartCoroutine(WaitForGun());
        InfoTextDisappear();
        StartCoroutine(MovieStart());
        yield return StartCoroutine(chatting.Chat(3f, "������� �����ӽ��ϴ�! ���߿� �����ϼ���!"));
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

    IEnumerator CameraMoveX(float amount, float duration, string method)
    {
        if (method == "flex") amount += mainCamera.transform.position.x;
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        mainCamera.transform.DOMoveX(amount, duration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(duration);
    }

    void CameraReturns()
    {
        mainCamera.GetComponent<CinemachineBrain>().enabled = true;
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
