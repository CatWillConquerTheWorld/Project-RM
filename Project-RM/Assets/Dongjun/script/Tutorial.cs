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

        //StartCoroutine(TutorialFlow());

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
        yield return StartCoroutine(yellerChat.Chat(2f, "다음!"));
        yield return waitOneSec;
        yellerChat.DisableChat();
        yield return StartCoroutine(PlayerMoveX(10f, 1, 2f));
        yield return new WaitForSeconds(0.2f);
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(4.1f, "자네가 다음 지원자인가?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.7f, "이렇게 건강한 사람들만 와서야 원..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6f,"아, 자네 앞에 많은 지원자가 있었다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.15f, "다 어떻게 됐냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.3f, "자네 순서까지 왔으면 알 법도 하지 않나?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4f, "잡소리는 여기까지 하고,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.65f, "이왕 왔으니 내 친절히 알려드리리다."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.9f,"자네는 지금부터 '부패한 성' 으로 들어간다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.1f, "그 안에는 썩은 고위 전사들과 마법사들,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.6f, "그리고 괴생명체들이 있다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.8f, "자네 임무는 부패한 왕을 찾아 없애는 것일세."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.1f, "어떻게 없애냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.3f, "아무래도 왕의 숨통을 끊어야겠지."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.05f, "맨몸으로 가냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.83f, "말 한번 잘했네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(7.1f, "장비는 줄걸세. 그리고 사용법도 친히 알려주지."));
        yield return StartCoroutine(WaitForUser());
        //카메라 무빙
        yield return StartCoroutine(CameraMoveX(10f, 1f, "flex"));
        yield return StartCoroutine(chatting.Chat(6.7f, "저 앞에 총이 보이는가? 가서 한번 잡아보게."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(MovieEnd());
        //카메라 무빙 해제
        yield return StartCoroutine(CameraMoveX(-10f, 1f, "flex"));
        //yield return StartCoroutine(CameraMoveX(-20f, 0.5f));
        CameraReturns();
        playerPlayerController.enabled = true;
        StartCoroutine(DisableWithDelay(chatting));
        InfoTextChange("방향키를 눌러 이동하세요.");
        InfoTextAppear();
        yield return StartCoroutine(WaitForGun());
        InfoTextDisappear();
        StartCoroutine(MovieStart());
        playerPlayerController.enabled = false;
        yield return StartCoroutine(PlayerMoveX(9f, -1, 4f));
        yield return StartCoroutine(PlayerMoveX(10f, 1, 4f));
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(4f, "그 총은 '리듬 건' 일세."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.5f, "음악이 흘러나오는 신기한 무기지."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.5f, "그 음악의 박자에 맞춰야 총탄을 내뱉는다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5f, "한번 시험삼아 써 보겠나?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3f, "그렇다면..."));
        yield return waitHalfSec;
        yield return StartCoroutine(CameraMoveX(20f, 1f, "flex"));
        StartCoroutine(TestRoomAppear());
        yield return StartCoroutine(CameraShake(3f, 0.1f, 40, false));
        yield return StartCoroutine(CameraShake(1f, 0.1f, 40, true));
        yield return waitHalfSec;
        yield return StartCoroutine(CameraMoveX(-20f, 1f, "flex"));
        CameraReturns();
        yield return StartCoroutine(chatting.Chat(7f, "저 앞에 있는 건물 안에 시험용 벌레들이 있다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(7f, "무기를 얕봤다간 큰코 다치니 작은 것부터 해보자고."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(7f, "그럼 건물 뒤로 가 있을테니 다 처치하고 오게."));
        yield return StartCoroutine(WaitForUser());
        chatting.DisableChat();
        yield return StartCoroutine(NPCMoves(37.5f));
        yield return StartCoroutine(MovieEnd());
        InfoTextChange("모든 벌레를 처치하세요.");
        InfoTextAppear();
        playerPlayerController.enabled = true;
        yield return WaitForElemenations(levelOneEnemies.transform.childCount);
        playerPlayerController.enabled = false;
        StartCoroutine(MovieStart());
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(5f, "잘 했네. 재능이 있구만."));
        yield return StartCoroutine(WaitForUser());
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
        InfoTextChange("F를 눌러 총을 잡으세요.");
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
