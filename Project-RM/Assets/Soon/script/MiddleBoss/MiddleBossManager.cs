using Cinemachine;
using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MiddleBossManager : MonoBehaviour
{
    public GameObject player;
    private GameObject playerGun;
    private PlayerController playerPlayerController;
    private Animator playerAnimator;

    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public GameObject boss;
    private Rigidbody2D rb;

    private bool isNext;
    public GameObject keyP;

    public CanvasGroup noteUIContainer;

    public float stageBPM;

    public TMP_Text readyText;
    public TMP_Text countText;
    private float readyTextCharacterSpace;
    public BoxCollider2D sceneStart;

    public CanvasGroup healthContainer;
    public CanvasGroup playerHealth;

    public BoxCollider2D doorCollider;
    public Animator doorAnimator;

    public Animator bossAnimator;

    public GameObject cutScene2Trigger;

    public AudioSource playerDown;
    public AudioSource middleBossDown;
    public AudioSource doorLock;
    public AudioSource wind;

    public RectTransform greyBG_up;
    public RectTransform greyBG_down;

    public static bool clear = false;

    void Start()
    {
        playerPlayerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        playerGun = player.transform.Find("Gun").gameObject;
        rb = boss.GetComponent<Rigidbody2D>();

        isNext = false;

        countText.enabled = false;
        readyTextCharacterSpace = 50f;
        readyText.enabled = false;

        healthContainer.gameObject.SetActive(false);

        noteUIContainer.alpha = 0f;

        playerHealth.alpha = 0f;

        doorAnimator.SetTrigger("DoorOpen");
        doorCollider.enabled = false;

        greyBG_up.DOAnchorPosY(810f, 0.3f).SetEase(Ease.InSine).SetDelay(0.5f);
        greyBG_down.DOAnchorPosY(-810f, 0.3f).SetEase(Ease.InSine).SetDelay(0.5f);
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
        playerAnimator.SetBool("isWalking", true);
        yield return StartCoroutine(PlayerMoveX(10f, 2.5f));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(CameraMoveY(-8f, 2f, "flex"));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CameraMoveY(8f, 0.5f, "flex"));
        CameraReturns();
        keyP.SetActive(true);
        yield return StartCoroutine(WaitForUser());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PlayerMoveX(11.5f, 4f));       
        yield return new WaitForSeconds(0.9f);        
        playerDown.Play();
        yield return new WaitForSeconds(0.4f);
        yield return StartCoroutine(CameraShake(1f, 0.5f, 40, true));

        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(CameraMoveY(-23.71721f, 0.5f, "notflex"));
        yield return new WaitForSeconds(0.5f);
        CameraReturns();
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(MovieEnd());
        playerPlayerController.enabled = true;

    }

    public IEnumerator MiddleBossEnter()
    {
        StartCoroutine(MovieStart());
        playerPlayerController.enabled = false;
        playerAnimator.SetBool("isWalking", false);
        doorAnimator.SetTrigger("DoorClose");
        doorLock.Play();
        doorCollider.enabled = true;
        playerPlayerController.Alert();
        yield return new WaitForSeconds(1.2f);
        playerPlayerController.DisableAlert();
        yield return StartCoroutine(PlayerMoveX(19.5f, 1.5f));

        rb.gravityScale = 10f;
        boss.SetActive(true);
        wind.Stop();
        yield return new WaitForSeconds(0.3f);
        middleBossDown.Play();
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CameraShake(1f, 0.5f, 40, false));
        yield return StartCoroutine(CameraShake(0.3f, 0.5f, 40, true));
        yield return new WaitForSeconds(0.5f);
        CameraReturns();
        rb.gravityScale = 1f;
        
        yield return new WaitForSeconds(1f);

        healthContainer.alpha = 0f;
        healthContainer.gameObject.SetActive(true);
        healthContainer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        noteUIContainer.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
        readyText.enabled = true;
        EnableHealth();
        EnableNote();
        StartCoroutine(MovieEnd());
        DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 300f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnComplete(() => readyText.enabled = false);
        readyText.DOFade(0f, 2f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(2f);
        countText.enabled = true;
        countText.text = "3";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.text = "2";
        yield return new WaitForSeconds(60f / stageBPM);
        LoadBMS.currentTime = 0d;
        LoadBMS.Instance.play_song("Venomous");
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
                yield return new WaitForSeconds((60f / stageBPM) * 8);
                LoadBMS.currentTime = -10000000d;
                CenterFrame.MusicFadeOut();
                StartCoroutine(GameOver.instance.GameOverAnim());
                yield break;
            }
            else if (GameObject.Find("Boss").GetComponent<Boss>().currentHealth <= 0)
            {
                CenterFrame.MusicFadeOut();
                noteUIContainer.DOFade(0f, 0.5f).SetEase(Ease.OutSine).OnComplete(() => noteUIContainer.gameObject.SetActive(false));
                healthContainer.DOFade(0f, 0.5f).SetEase(Ease.OutSine);
                boss.GetComponent<Boss>().Die();
                doorAnimator.SetTrigger("DoorOpen");
                doorCollider.enabled = false;
                clear = true;
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

    IEnumerator CameraMoveY(float amount, float duration, string method)
    {
        if (method == "flex") amount += Camera.main.transform.position.y;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        Camera.main.transform.DOMoveY(amount, duration).SetEase(Ease.Linear);
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


    void EnableHealth()
    {
        playerHealth.DOFade(1f, 0.5f).SetEase(Ease.OutSine);
    }
}
