using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;
using UnityEngine.UIElements;
using System.Globalization;
using Unity.VisualScripting;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public GameObject player;
    private GameObject playerGun;
    private SpriteRenderer playerGunSpriteRenderer;
    private PlayerController playerPlayerController;
    private SpriteRenderer playerSpriteRenderer;
    private Animator playerAnimator;

    public GameObject chatManagerYeller;
    public GameObject tutorialNPC;
    public GameObject chatManagerTutorialNPC;

    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public GameObject infoText;

    public GameObject gun;

    public GameObject testRoom;
    public GameObject levelOneEnemies;
    public GameObject levelTwoEnemies;
    public int deadEnemies;

    private RectTransform infoTextRect;
    private TextMeshProUGUI infoTextTMP;

    private Chatting yellerChat;
    private Chatting chatting;

    private WaitForSeconds waitOneSec;
    private WaitForSeconds waitHalfSec;
    private WaitForSeconds waitForDisableChatter;

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

        infoTextRect = infoText.GetComponent<RectTransform>();
        infoTextTMP = infoText.GetComponent<TextMeshProUGUI>();

        waitOneSec = new WaitForSeconds(1);
        waitHalfSec = new WaitForSeconds(0.5f);
        waitForDisableChatter = new WaitForSeconds(0.7f);

        canHoldGun = false;
        holdingGun = false;

        for (int i = 0; i < levelOneEnemies.transform.childCount; i++)
        {
            levelOneEnemies.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            Destroy(levelOneEnemies.transform.GetChild(i).GetComponent<Rigidbody2D>());
        }
        for (int i = 0; i < levelTwoEnemies.transform.childCount; i++) 
        {
            levelTwoEnemies.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            Destroy(levelTwoEnemies.transform.GetChild(i).GetComponent<Rigidbody2D>());
            levelTwoEnemies.transform.GetChild(i).GetComponent<EnemyController>().enabled = false;
            levelTwoEnemies.transform.GetChild(i).GetComponent<Cage>().enabled = false;
        }

        StartCoroutine(TutorialFlow());

        isNext = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            isNext = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(TestRoomAppear());
        }

        if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(WakeUpLevelTwoEnemies());
        if (Input.GetKeyDown (KeyCode.W)) LevelTwoEnemiesGiveSettings();
    }

    IEnumerator TutorialFlow()
    {
        playerPlayerController.enabled = false;
        playerGun.SetActive(false);
        chatting.DisableChat();
        yellerChat.DisableChat();
        yield return waitOneSec;
        yellerChat.EnableChat();
        yield return StartCoroutine(yellerChat.Chat(1.7f, "����!"));
        yield return waitOneSec;
        yellerChat.DisableChat();
        yield return StartCoroutine(PlayerMoveX(10f, 2f));
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
        StartCoroutine(DisableWithDelay(chatting));
        InfoTextChange("����Ű�� ���� �̵��ϼ���.");
        InfoTextAppear();
        yield return StartCoroutine(WaitForGun());
        InfoTextDisappear();
        StartCoroutine(MovieStart());
        playerPlayerController.enabled = false;
        yield return StartCoroutine(PlayerMoveX(9.5f, 4f));
        yield return StartCoroutine(PlayerMoveX(10f, 4f));
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(4f, "�� ���� '���� ��' �ϼ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.2f, "������ �귯������ �ű��� ������."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.7f, "�� ������ ���ڿ� ����� ��ź�� ����´ٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.2f, "�ѹ� ������ �� ���ڳ�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.35f, "�׷��ٸ�..."));
        yield return waitHalfSec;
        yield return StartCoroutine(CameraMoveX(20f, 1f, "flex"));
        StartCoroutine(TestRoomAppear());
        yield return StartCoroutine(CameraShake(3f, 0.1f, 40, false));
        yield return StartCoroutine(CameraShake(1f, 0.1f, 40, true));
        yield return waitHalfSec;
        yield return StartCoroutine(CameraMoveX(-20f, 1f, "flex"));
        CameraReturns();
        yield return StartCoroutine(chatting.Chat(7.2f, "�� �տ� �ִ� �ǹ� �ȿ� ����� �������� �ִٳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(7.55f, "���⸦ ��ôٰ� ū�� ��ġ�� ���� �ͺ��� �غ��ڰ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.9f, "�׷� �ǹ� �ڷ� �� �����״� �� óġ�ϰ� ����."));
        yield return StartCoroutine(WaitForUser());
        chatting.DisableChat();
        yield return StartCoroutine(NPCMoves(40f));
        yield return StartCoroutine(MovieEnd());
        InfoTextChange("��� ������ óġ�ϼ���.");
        InfoTextAppear();
        playerPlayerController.enabled = true;
        yield return WaitForElemenations(levelOneEnemies.transform.childCount);
        playerPlayerController.enabled = false;
        StartCoroutine(MovieStart());
        yield return waitHalfSec;
        yield return PlayerMoveX(38f, 3f);
        chatting.EnableChat();
        InfoTextDisappear();
        yield return StartCoroutine(chatting.Chat(3.95f, "�� �߳�. ����� �ֱ���."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.6f, "�׷� �ٷ� ���� �ܰ�� ����."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(CameraMoveX(-5f, 0.5f, "flex"));
        yield return waitOneSec;
        yield return StartCoroutine(WakeUpLevelTwoEnemies());
        yield return waitOneSec;
        yield return StartCoroutine(CameraMoveX(5f, 0.5f, "flex"));
        CameraReturns();
        yield return StartCoroutine(chatting.Chat(8.4f, "�� ���ù����� �ֵ��� '������' ��� �Ҹ��� ������ü���."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4f, "�̹����� ���� �����ɼ�."));
        yield return StartCoroutine(WaitForUser());
        StartCoroutine(DisableWithDelay(chatting));
        LevelTwoEnemiesGiveSettings();
        yield return StartCoroutine(MovieEnd());
        InfoTextAppear();
        InfoTextChange("��� �������� óġ�ϼ���.");
        playerPlayerController.enabled = true;
        yield return WaitForElemenations(levelTwoEnemies.transform.childCount);
        playerPlayerController.enabled = false;
        StartCoroutine(MovieStart());
        InfoTextDisappear();
        yield return PlayerMoveX(38f, 3f);
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(3.1f, "���� ��ġ���°�?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6f, "�������� �ű⿡ ������ �� �ϰڱ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.8f, "�׷� �� ���� ������ ������ ����."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.1f, "�ڳ׶�� �� �����ҰŶ�� �ϳ�."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.4f, "����� ����."));
        yield return StartCoroutine(WaitForUser());
        StartCoroutine(MovieEnd());
        StartCoroutine(DisableWithDelay(chatting));
        playerPlayerController.enabled = true;
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        while (!isNext) yield return null;
    }

    IEnumerator DisableWithDelay(Chatting target)
    {
        yield return waitForDisableChatter;
        target.DisableChat();
        yield return null;
    }

    IEnumerator PlayerMoveX(float destinationX, float moveSpeed)
    {
        int direction = 0;
        if (player.transform.position.x < destinationX) 
        {
            direction = 1;
            player.transform.localScale = new Vector3(3, 3, 0);
            playerGun.transform.position = player.transform.position + new Vector3(-0.1f, -0.275f, 0);
        }
        else if (player.transform.position.x > destinationX)
        {
            direction = -1;
            player.transform.localScale = new Vector3(-3, 3, 0);
            playerGun.transform.position = player.transform.position + new Vector3(0.1f, -0.275f, 0);
        }
        else print("Wrong Direction!");
        playerAnimator.SetBool("isWalking", true);
        playerAnimator.SetBool("isJump", false);
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
        yield return null;
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
        if (method == "flex") amount += Camera.main.transform.position.x;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        Camera.main.transform.DOMoveX(amount, duration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(duration);
    }

    IEnumerator CameraShake(float duration, float amount, int vibrato, bool isFadeOut)
    {
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        Camera.main.DOShakePosition(duration, amount, vibrato, 90, isFadeOut);
        yield return new WaitForSeconds(duration);
    }

    void CameraReturns()
    {
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
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

    IEnumerator TestRoomAppear()
    {
        while (testRoom.transform.position.y != 8f)
        {
            for (int i = 0; i < levelOneEnemies.transform.childCount; i++)
            {
                GameObject levelOneEnemy = levelOneEnemies.transform.GetChild(i).gameObject;
                levelOneEnemies.transform.GetChild(i).gameObject.transform.position = Vector3.MoveTowards(levelOneEnemy.transform.position, levelOneEnemy.transform.position + new Vector3(0,8,0), 3f * Time.deltaTime);
            }
            for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
            {
                GameObject levelTwoEnemy = levelTwoEnemies.transform.GetChild(i).gameObject;
                levelTwoEnemies.transform.GetChild(i).gameObject.transform.position = Vector3.MoveTowards(levelTwoEnemy.transform.position, levelTwoEnemy.transform.position + new Vector3(0,8,0), 3f * Time.deltaTime);
            }
            testRoom.transform.position = Vector3.MoveTowards(testRoom.transform.position, new Vector3(testRoom.transform.position.x, 8f, testRoom.transform.position.z), 3f * Time.deltaTime);
            yield return null;
        }
        for (int i = 0; i < levelOneEnemies.transform.childCount; i++)
        {
            GameObject levelOneEnemy = levelOneEnemies.transform.GetChild(i).gameObject;
            levelOneEnemy.AddComponent<Rigidbody2D>();
            levelOneEnemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            levelOneEnemy.GetComponent<Rigidbody2D>().freezeRotation = true;
            levelOneEnemy.GetComponent<BoxCollider2D>().enabled = true; 
        }
        for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
        {
            GameObject levelTwoEnemy = levelTwoEnemies.transform.GetChild(i).gameObject;
            levelTwoEnemy.AddComponent<Rigidbody2D>();
            levelTwoEnemy.GetComponent<Rigidbody2D>().freezeRotation = true;
            levelTwoEnemy.GetComponent<BoxCollider2D>().enabled = true;
        }
        yield return null;
    }

    IEnumerator NPCMoves(float destinationX)
    {
        tutorialNPC.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetEase(Ease.Linear);
        yield return waitHalfSec;
        tutorialNPC.transform.position = new Vector3(destinationX, tutorialNPC.transform.position.y, tutorialNPC.transform.position.z);
        Color temp = tutorialNPC.GetComponent<SpriteRenderer>().color;
        temp.a = 1;
        tutorialNPC.GetComponent<SpriteRenderer>().color = temp;
        yield return null;
    }

    IEnumerator WaitForElemenations(int amount)
    {
        while (deadEnemies < amount) yield return null;
        deadEnemies = 0;
        yield return null;
    }

    IEnumerator WakeUpLevelTwoEnemies()
    {
        for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
        {
            levelTwoEnemies.transform.GetChild(i).GetComponent<Animator>().SetBool("initiated", true);
            levelTwoEnemies.transform.GetChild(i).GetComponent<EnemyController>().enabled = true;
            levelTwoEnemies.transform.GetChild(i).GetComponent<Cage>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    void LevelTwoEnemiesGiveSettings()
    {
        for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
        {
            levelTwoEnemies.transform.GetChild(i).tag = "Enemy";
            levelTwoEnemies.transform.GetChild(i).gameObject.layer = 7;
            levelTwoEnemies.transform.GetChild(i).transform.Find("attackCollider").tag = "EnemyBullet";
        }
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
