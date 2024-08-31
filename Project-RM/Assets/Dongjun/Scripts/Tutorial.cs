using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public GameObject player;
    private GameObject playerGun;
    private PlayerController playerPlayerController;
    private Animator playerAnimator;

    public GameObject chatManagerYeller;
    public GameObject tutorialNPC;
    public GameObject chatManagerTutorialNPC;

    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public GameObject infoText;
    public GameObject keyP;
    public CanvasGroup arrowContainer;

    public GameObject gun;

    public GameObject[] testRoom;
    public GameObject levelOneEnemies;
    public GameObject levelTwoEnemies;
    public GameObject doorLeft;
    public GameObject doorRight;
    public int deadEnemies;

    private RectTransform infoTextRect;
    private TextMeshProUGUI infoTextTMP;

    private Chatting yellerChat;
    private Chatting chatting;

    private WaitForSeconds waitOneSec;
    private WaitForSeconds waitHalfSec;
    private WaitForSeconds waitForDisableChatter;

    public CanvasGroup noteUIContainer;

    //skipping to next description
    private bool isNext;

    //checks if player is in the interactive gun's field
    public bool canHoldGun;
    public bool holdingGun;

    public bool isDoorOpened;
    public float stageBPM;

    public GameObject keysInfo;

    public TMP_Text readyText;
    public TMP_Text countText;
    public float readyTextCharacterSpace;

    public GameObject pauseButton;

    public BoxCollider2D blocker;

    public BoxCollider2D blocker2;

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
        playerAnimator = player.GetComponent<Animator>();

        playerGun = player.transform.Find("Gun").gameObject;

        yellerChat = chatManagerYeller.GetComponent<Chatting>();
        chatting = chatManagerTutorialNPC.GetComponent<Chatting>();

        infoTextRect = infoText.GetComponent<RectTransform>();
        infoTextTMP = infoText.GetComponent<TextMeshProUGUI>();

        waitOneSec = new WaitForSeconds(1);
        waitHalfSec = new WaitForSeconds(0.5f);
        waitForDisableChatter = new WaitForSeconds(0.7f);
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
        yellerChat.DisableChat();
        chatting.DisableChat();
        //StartCoroutine(TutorialFlow());

        readyText.enabled = false;
        countText.enabled = false;

        noteUIContainer.alpha = 0f;
        //noteUIContainer.gameObject.SetActive(false);
        isDoorOpened = false;


        blocker.enabled = true;

        isNext = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) isNext = true;
        //if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(TestRoomAppear());
    }

    public IEnumerator TutorialFlow()
    {
        Pause.isOKToPause = false;
        yield return waitOneSec;
        yellerChat.EnableChat();
        yield return StartCoroutine(yellerChat.Chat(1.65f, "다음!"));
        yield return waitOneSec;
        yellerChat.DisableChat();
        yield return StartCoroutine(PlayerMoveX(10f, 2f));
        yield return new WaitForSeconds(0.2f);
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(3.95f, "자네가 다음 지원자인가?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.4f, "이렇게 건강한 사람들만 와서야 원..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.7f,"아, 자네 앞에 많은 지원자가 있었다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.15f, "다 어떻게 됐냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.1f, "자네 순서까지 왔으면 알 법도 하지 않나?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.9f, "잡소리는 여기까지 하고,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.55f, "이왕 왔으니 내 친절히 알려드리리다."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.7f,"자네는 지금부터 '부패한 성' 으로 들어간다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.9f, "그 안에는 썩은 고위 전사들과 마법사들,"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.3f, "그리고 괴생명체들이 있다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.55f, "자네 임무는 부패한 왕을 찾아 없애는 것일세."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.95f, "어떻게 없애냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.1f, "아무래도 왕의 숨통을 끊어야겠지."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.05f, "맨몸으로 가냐고?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.78f, "말 한번 잘했네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.8f, "장비는 줄걸세. 그리고 사용법도 친히 알려주지."));
        yield return StartCoroutine(WaitForUser());
        //카메라 무빙
        yield return StartCoroutine(CameraMoveX(10f, 1f, "flex"));
        yield return StartCoroutine(chatting.Chat(6.4f, "저 앞에 총이 보이는가? 가서 한번 잡아보게."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(MovieEnd());
        //카메라 무빙 해제
        yield return StartCoroutine(CameraMoveX(-10f, 1f, "flex"));
        //yield return StartCoroutine(CameraMoveX(-20f, 0.5f));
        CameraReturns();
        playerPlayerController.enabled = true;
        StartCoroutine(DisableWithDelay(chatting));
        ArrowAppear();
        yield return StartCoroutine(WaitForGun());
        ArrowDisappear();
        StartCoroutine(MovieStart());
        playerPlayerController.enabled = false;
        yield return StartCoroutine(PlayerMoveX(9.5f, 4f));
        yield return StartCoroutine(PlayerMoveX(10f, 4f));
        chatting.EnableChat();
        blocker.enabled = false;
        yield return StartCoroutine(chatting.Chat(3.6f, "그 총은 '리듬 건' 일세."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.2f, "음악이 흘러나오는 신기한 무기지."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.4f, "그 음악의 박자에 맞춰야 총탄을 내뱉는다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.05f, "한번 시험삼아 써 보겠나?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.25f, "그렇다면..."));
        yield return waitHalfSec;
        yield return StartCoroutine(CameraMoveX(20f, 1f, "flex"));
        StartCoroutine(TestRoomAppear());
        yield return StartCoroutine(CameraShake(5f, 0.1f, 40, false));
        yield return StartCoroutine(CameraShake(1f, 0.1f, 40, true));
        yield return waitHalfSec;
        yield return StartCoroutine(CameraMoveX(-20f, 1f, "flex"));
        CameraReturns();
        yield return StartCoroutine(chatting.Chat(7.2f, "저 앞에 있는 건물 안에 시험용 벌레들이 있다네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(7.55f, "무기를 얕봤다간 큰코 다치니 작은 것부터 해보자고."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(6.7f, "그럼 건물 뒤로 가 있을테니 다 처치하고 오게."));
        yield return StartCoroutine(WaitForUser());
        chatting.DisableChat();
        yield return StartCoroutine(NPCMoves(40f));
        yield return StartCoroutine(MovieEnd());
        //Pause.isOKToPause = true;
        keysInfo.GetComponent<RectTransform>().DOAnchorPosY(-50f, 0.5f).SetEase(Ease.OutSine);
        playerPlayerController.enabled = true;
        yield return StartCoroutine(WaitForDoorOpen());
        playerPlayerController.enabled = false;
        readyText.enabled = true;
        EnableNote();
        DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 300f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnComplete(() => readyText.enabled = false);
        readyText.DOFade(0f, 2f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(2f);
        //EnemyNoteManager.isMusicStart = true;
        countText.enabled = true;
        countText.text = "3";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        countText.text = "2";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        LoadBMS.Instance.play_song("tutorialLevel1");
        countText.text = "1";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        countText.text = "GO!";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        countText.enabled = false;
        playerPlayerController.enabled = true;
        yield return StartCoroutine(WaitForElemenations(levelOneEnemies.transform.childCount, 1));
        keysInfo.GetComponent<RectTransform>().DOAnchorPosY(150f, 0.5f).SetEase(Ease.OutSine);
        playerPlayerController.enabled = false;
        StartCoroutine(MovieStart());
        CenterFrame.MusicFadeOut();
        //Pause.isOKToPause = false;
        //EnemyNoteManager.isMusicStart = false;
        DisableNote();
        readyText.enabled = false;
        countText.enabled = false;
        isDoorOpened = false;
        yield return waitHalfSec;
        yield return PlayerMoveX(38f, 3f);
        chatting.EnableChat();
        InfoTextDisappear();
        //TestRoomDoor.Reset();
        yield return StartCoroutine(chatting.Chat(3.85f, "잘 했네. 재능이 있구만."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.5f, "그럼 바로 다음 단계로 가지."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(CameraMoveX(-5f, 0.5f, "flex"));
        yield return waitOneSec;
        yield return StartCoroutine(WakeUpLevelTwoEnemies());
        yield return waitOneSec;
        yield return StartCoroutine(CameraMoveX(5f, 0.5f, "flex"));
        CameraReturns();
        yield return StartCoroutine(chatting.Chat(8.25f, "저 무시무시한 애들은 '케이지' 라고 불리는 괴생명체라네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(3.9f, "이번에는 쉽지 않을걸세."));
        yield return StartCoroutine(WaitForUser());
        StartCoroutine(DisableWithDelay(chatting));
        yield return StartCoroutine(MovieEnd());
        InfoTextAppear();
        InfoTextChange("모든 케이지를 처치하세요.");
        //Pause.isOKToPause = true;
        playerPlayerController.enabled = false;
        EnableNote();
        readyText.enabled = true;
        readyText.DOFade(1f, 0.00001f);
        readyTextCharacterSpace = 50f;
        readyText.characterSpacing = readyTextCharacterSpace;
        DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 300f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnComplete(() => readyText.enabled = false);
        readyText.DOFade(0f, 2f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(2f);
        countText.enabled = true;
        countText.text = "3";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        countText.text = "2";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        LoadBMS.Instance.play_song("tutorialLevel2");
        LoadBMS.currentTime = 0d;
        countText.text = "1";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        countText.text = "GO!";
        yield return new WaitForSeconds(60f / bpmManager.instance.bpm);
        countText.enabled = false;
        readyText.enabled = false;
        GiveCagesEnemyController();
        playerPlayerController.enabled = true;
        yield return WaitForElemenations(levelTwoEnemies.transform.childCount, 2);
        //Pause.isOKToPause = false;
        CenterFrame.MusicFadeOut();
        DisableNote();
        playerPlayerController.enabled = false;
        StartCoroutine(MovieStart());
        InfoTextDisappear();
        yield return waitHalfSec;
        yield return PlayerMoveX(38f, 3f);
        chatting.EnableChat();
        yield return StartCoroutine(chatting.Chat(3.1f, "벌써 해치웠는가?"));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.9f, "이정도면 거기에 들어가서도 잘 하겠군."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(5.6f, "그럼 그 총을 가지고 안으로 들어가게."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(4.9f, "자네라면 꼭 성공할거라고 믿네."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(chatting.Chat(2.4f, "행운을 빌지."));
        yield return StartCoroutine(WaitForUser());
        StartCoroutine(MovieEnd());
        StartCoroutine(DisableWithDelay(chatting));
        playerPlayerController.enabled = true;
        Pause.isOKToPause = true;
        pauseButton.gameObject.SetActive(true);
        PlayerPrefs.SetInt("tutorialCleared", 1);
        blocker2.enabled = false;
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        keyP.SetActive(true);
        while (!isNext) yield return null;
        keyP.SetActive(false);
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

    public IEnumerator MovieEnd()
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
        while (!holdingGun) yield return null;
        gun.SetActive(false);
        playerGun.SetActive(true);
        yield return null;
    }

    IEnumerator TestRoomAppear()
    {
        while (testRoom[0].transform.position.y != 12f)
        {
            for (int i = 0; i < levelOneEnemies.transform.childCount; i++)
            {
                GameObject levelOneEnemy = levelOneEnemies.transform.GetChild(i).gameObject;
                levelOneEnemies.transform.GetChild(i).gameObject.transform.position = Vector3.MoveTowards(levelOneEnemy.transform.position, levelOneEnemy.transform.position + new Vector3(0,13,0), 3f * Time.deltaTime);
            }
            for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
            {
                GameObject levelTwoEnemy = levelTwoEnemies.transform.GetChild(i).gameObject;
                levelTwoEnemies.transform.GetChild(i).gameObject.transform.position = Vector3.MoveTowards(levelTwoEnemy.transform.position, levelTwoEnemy.transform.position + new Vector3(0,13,0), 3f * Time.deltaTime);
            }
            for (int i = 0; i < testRoom.Length; i++)
            {
                testRoom[i].transform.position = Vector3.MoveTowards(testRoom[i].transform.position, new Vector3(testRoom[i].transform.position.x, 12f, testRoom[i].transform.position.z), 3f * Time.deltaTime);
            }
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, doorLeft.transform.position + new Vector3(0, 13, 0), 3f * Time.deltaTime);
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, doorRight.transform.position + new Vector3(0, 13, 0), 3f * Time.deltaTime);
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

    IEnumerator WaitForDoorOpen()
    {
        while (!isDoorOpened) yield return null;
        yield return null;
    }

    IEnumerator WaitForElemenations(int amount, int level)
    {
        while (deadEnemies < amount)
        {
            if (LoadBMS.Instance.isEnded)
            {
                LoadBMS.currentTime = 0d;
                if (level == 1)
                {
                    LoadBMS.Instance.play_song("tutorialLevel1");
                }
                else if (level == 2)
                {
                    LoadBMS.Instance.play_song("tutorialLevel2");
                }
            }
            yield return null;
        }
        yield return null;
        deadEnemies = 0;
        yield return null;
    }

    IEnumerator WakeUpLevelTwoEnemies()
    {
        for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
        {
            levelTwoEnemies.transform.GetChild(i).GetComponent<Animator>().SetBool("initiated", true);
            levelTwoEnemies.transform.GetChild(i).GetComponent<EnemyController>().enabled = false;
            levelTwoEnemies.transform.GetChild(i).GetComponent<Cage>().enabled = true;
            levelTwoEnemies.transform.GetChild(i).tag = "Enemy";
            levelTwoEnemies.transform.GetChild(i).gameObject.layer = 7;
            levelTwoEnemies.transform.GetChild(i).transform.Find("attackCollider").tag = "EnemyBullet";
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    void GiveCagesEnemyController()
    {
        for (int i = 0; i < levelTwoEnemies.transform.childCount; i++)
        {
            levelTwoEnemies.transform.GetChild(i).GetComponent<EnemyController>().enabled = true;
        }
    }

    void EnableNote()
    {
        noteUIContainer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
    }

    void DisableNote()
    {
        noteUIContainer.DOFade(0f, 0.5f).SetEase(Ease.InSine); ;
    }

    void ArrowAppear()
    {
        arrowContainer.DOFade(1f, 0.1f).SetEase(Ease.OutSine);
    }

    void ArrowDisappear()
    {
        arrowContainer.DOFade(0f, 0.1f).SetEase(Ease.OutSine);
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
