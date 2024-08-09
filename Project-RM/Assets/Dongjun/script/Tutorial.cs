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
        yield return StartCoroutine(WaitForUser());
        //카메라 무빙
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "저 앞에 총이 보이는가? 가서 한번 잡아보게."));
        yield return StartCoroutine(MovieEnd());
        //카메라 무빙 해제
        InfoTextChange("방향키를 눌러 이동하세요.");
        InfoTextAppear();
        yield return StartCoroutine(WaitForGun());
        InfoTextDisappear();
        StartCoroutine(MovieStart());
        yield return StartCoroutine(chatting.Chat(3f, 1.5f, "여기까진 순조롭습니다! 개발에 정진하세요!"));
        //카메라 무빙
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
        InfoTextChange("F를 눌러 총을 잡으세요.");
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
