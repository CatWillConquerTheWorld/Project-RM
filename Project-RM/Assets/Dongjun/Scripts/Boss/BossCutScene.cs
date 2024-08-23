using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class BossCutScene : MonoBehaviour
{
    public GameObject player;
    private GameObject playerGun;
    private PlayerController playerPlayerController;
    private Animator playerAnimator;

    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public GameObject boss;
    private Chatting bossChat;

    private bool isNext;
    public GameObject keyP;

    public CanvasGroup noteUIContainer;

    public float stageBPM;

    public TMP_Text readyText;
    public TMP_Text countText;
    private float readyTextCharacterSpace;

    public BoxCollider2D wall;

    public CanvasGroup healthContainer;

    void Start()
    {
        playerPlayerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        playerGun = player.transform.Find("Gun").gameObject;

        bossChat = boss.transform.Find("ChatManager").GetComponent<Chatting>();

        isNext = false;

        countText.enabled = false;
        readyTextCharacterSpace = 50f;
        readyText.enabled = false;

        healthContainer.gameObject.SetActive(false);

        noteUIContainer.gameObject.SetActive(false);

        wall.enabled = false;

        bossChat.DisableChat();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) isNext = true;
        //if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(TestRoomAppear());
    }

    IEnumerator CutScene()
    {
        StartCoroutine(MovieStart());
        playerPlayerController.enabled = false;
        playerAnimator.SetBool("isWaking", true);
        yield return StartCoroutine(PlayerMoveX(-1.55f, 2.5f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CameraMoveX(3f, 1f, "flex"));
        yield return new WaitForSeconds(0.5f);
        bossChat.EnableChat();
        yield return StartCoroutine(bossChat.Chat(4f, "... 또 나를 죽이러 왔는가..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(4f, "이번에도 초짜인 것 같군..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(2.3f, "...그렇다면..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(4.2f, "...다시는 발을 못 들이도록..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(3.5f, "...목숨을 끊어주겠다..."));
        yield return StartCoroutine(WaitForUser());
        bossChat.DisableChat();
        StartCoroutine(MovieEnd());
        yield return StartCoroutine(CameraMoveX(-3f, 1f, "flex"));
        CameraReturns();
        healthContainer.alpha = 0f;
        healthContainer.gameObject.SetActive(true);
        healthContainer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        noteUIContainer.gameObject.SetActive(true);
        noteUIContainer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        wall.enabled = true;
        readyText.enabled = true;
        DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 300f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnComplete(() => readyText.enabled = false);
        readyText.DOFade(0f, 2f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(2f);
        countText.enabled = true;
        countText.text = "3";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.text = "2";
        yield return new WaitForSeconds(60f / stageBPM);
        LoadBMS.currentTime = 0d;
        LoadBMS.Instance.play_song("deads");
        countText.text = "1";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.text = "GO!";
        playerPlayerController.enabled = true;
        yield return new WaitForSeconds(60f / stageBPM);
        countText.enabled = false;
        playerPlayerController.enabled = true;
        yield return StartCoroutine(WaitForElemenation());
    }

    IEnumerator WaitForUser()
    {
        isNext = false;
        keyP.SetActive(true);
        while (!isNext) yield return null;
        keyP.SetActive(false);
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

    IEnumerator WaitForElemenation()
    {
        while (true)
        {
            if (player.GetComponent<PlayerController>().GetIsDead())
            {
                LoadBMS.currentTime = -10000000d;
                CenterFrame.MusicFadeOut();
                StartCoroutine(GameOver.instance.GameOverAnim());
                yield break;

            }
            else if (LoadBMS.Instance.isEnded)
            {
                LoadBMS.currentTime = -10000000d;
                CenterFrame.MusicFadeOut();
                StartCoroutine(GameOver.instance.GameOverAnim());
                yield break;
            }
            else if (GameObject.Find("middleBoss").GetComponent<MiddleBoss>().currentHealth <= 0)
            {
                CenterFrame.MusicFadeOut();
                noteUIContainer.DOFade(0f, 0.5f).SetEase(Ease.OutSine).OnComplete(() => noteUIContainer.gameObject.SetActive(false));
                healthContainer.DOFade(0f, 0.5f).SetEase(Ease.OutSine);
                boss.GetComponent<MiddleBoss>().Die();
                yield break;
            }
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(CutScene());
        }
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

    void EnableNote()
    {
        noteUIContainer.gameObject.SetActive(true);
        noteUIContainer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
    }

    void DisableNote()
    {
        noteUIContainer.DOFade(0f, 0.5f).SetEase(Ease.InSine).OnComplete(() => noteUIContainer.gameObject.SetActive(false));
    }

}
