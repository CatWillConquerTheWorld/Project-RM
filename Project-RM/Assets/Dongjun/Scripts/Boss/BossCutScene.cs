using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    void Start()
    {
        playerPlayerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        playerGun = player.transform.Find("Gun").gameObject;

        bossChat = boss.transform.Find("ChatManager").GetComponent<Chatting>();

        isNext = false;

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
        yield return StartCoroutine(bossChat.Chat(5f, "... 또 나를 죽이러 왔는가..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(5f, "이번에도 초짜인 것 같군..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(5f, "...그렇다면..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(5f, "...다시는 발을 못 들이도록..."));
        yield return StartCoroutine(WaitForUser());
        yield return StartCoroutine(bossChat.Chat(5f, "...목숨을 끊어주겠다."));
        yield return StartCoroutine(WaitForUser());
        bossChat.DisableChat();
        StartCoroutine(MovieEnd());
        yield return StartCoroutine(CameraMoveX(-3f, 1f, "flex"));
        CameraReturns();
        yield return new WaitForSeconds(0.5f);
        playerPlayerController.enabled = true;
        yield return StartCoroutine(WaitForElemenation());
        bossChat.EnableChat();
        yield return StartCoroutine(bossChat.Chat(5f, "이겼닭! 오늘 저녁은 치킨이닭!"));
        yield return StartCoroutine(WaitForUser());
        boss.GetComponent<MiddleBoss>().Die();
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
            if (playerPlayerController.hp <= 0)
            {
                print("Dead");
                yield break;
            }
            else if (boss.GetComponent<MiddleBoss>().currentHealth <= 0)
            {
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
