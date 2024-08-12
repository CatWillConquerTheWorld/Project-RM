using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;
using UnityEngine.UIElements;
using System.Globalization;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public GameObject player;
    private GameObject playerGun;
    private SpriteRenderer playerGunSpriteRenderer;
    private PlayerController playerPlayerController;
    private SpriteRenderer playerSpriteRenderer;
    private Animator playerAnimator; 

    public Camera mainCamera;
    public GameObject chatManagerTutorialNPC;
    public GameObject chatManagerYeller;
    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public GameObject infoText;

    public GameObject gun;


    private RectTransform infoTextRect;
    private TextMeshProUGUI infoTextTMP;

    private Chatting yellerChat;
    private Chatting chatting;
    private WaitForSeconds waitOneSec;

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
        playerPlayerController = player.GetComponent<PlayerController>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        playerAnimator = player.GetComponent<Animator>();

        playerGun = player.transform.Find("Gun").gameObject;
        playerGunSpriteRenderer = playerGun.GetComponent<SpriteRenderer>();

        yellerChat = chatManagerYeller.GetComponent<Chatting>();
        chatting = chatManagerTutorialNPC.GetComponent<Chatting>();
        waitOneSec = new WaitForSeconds(1);

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
        playerGun.SetActive(false);
        chatting.DisableChat();
        yellerChat.DisableChat();
        yield return waitOneSec;
        yellerChat.EnableChat();
        playerPlayerController.enabled = false;
        yield return StartCoroutine(yellerChat.Chat(2f, "����!"));
        yield return waitOneSec;
        yellerChat.DisableChat();
        yield return StartCoroutine(PlayerMoveX(10f, 1, 2f));
        yield return new WaitForSeconds(0.2f);
        chatting.EnableChat();
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
        playerPlayerController.enabled = true;
        InfoTextChange("����Ű�� ���� �̵��ϼ���.");
        InfoTextAppear();
        yield return StartCoroutine(WaitForGun());
        InfoTextDisappear();
        StartCoroutine(MovieStart());
        playerPlayerController.enabled = false;
        yield return StartCoroutine(PlayerMoveX(9f, -1, 4f));
        yield return StartCoroutine(PlayerMoveX(10f, 1, 4f));
        //ī�޶� ����
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

    IEnumerator PlayerMoveX(float destinationX, float direction, float moveSpeed)
    {
        if (direction == 1)
        {
            playerSpriteRenderer.flipX = false;
            playerGunSpriteRenderer.flipX = false;
            playerGun.transform.position = player.transform.position + new Vector3(-0.1f, -0.275f, 0);
        }
        else if (direction == -1)
        {
            playerSpriteRenderer.flipX = true;
            playerGunSpriteRenderer.flipX = true;
            playerGun.transform.position = player.transform.position + new Vector3(0.1f, -0.275f, 0);
        }
        else print("Wrong Direction!");
        playerAnimator.SetBool("isWalking", true);
        playerAnimator.SetFloat("SpeedHandler", moveSpeed / 2.5f);
        while (true)
        {
            if (direction == 1 && player.transform.position.x > destinationX) break;
            if (direction == -1 && player.transform.position.x < destinationX) break;
            player.transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetFloat("SpeedHandler", 1f);
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
        playerGun.SetActive(true);
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
