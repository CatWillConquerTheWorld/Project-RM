using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public Animator portalAnimator;
    private bool isOnPortal = false; // 플레이어가 포탈 A 위에 있는지 여부
    private bool isNoEnemy = false;
    public Material material;
    public GameObject KeyF;
    public GameObject[] enemies;
    public Stage1 stage1;
    public bool isMiddleBoss;
    public bool isStage2;

    public GameObject player;
    private GameObject playerGun;
    private Animator playerAnimator;
    public GameObject greyBG_up;
    public GameObject greyBG_down;

    private void Start()
    {
        playerAnimator = player.GetComponent<Animator>();

        playerGun = player.transform.Find("Gun").gameObject;
        //stage1 = GetComponent<Stage1>();
    }
    void Update()
    {

        if (isMiddleBoss)
        {

            ApplyMaterialToPortal();
            if (isOnPortal && MiddleBossManager.clear)
            {
                KeyF.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(NextScene());
                }
            }

        }
        else if (isStage2)
        {

            if (isOnPortal && Stage2.waveClear)
            {
                ApplyMaterialToPortal();
                KeyF.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(GoToBoss());
                }
            }
            else KeyF.SetActive(false);
        }
        else
        {
            

            if (Stage1.isSpawn == true)
            {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");

                if (enemies.Length == 0)
                {
                    isNoEnemy = true;
                    ApplyMaterialToPortal();
                }
            }




            if (isOnPortal && stage1.isOkToGo)
            {
                KeyF.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(NextScene());
                }
            }
            else KeyF.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 포탈에 들어왔을 때
        {
            isOnPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 포탈에서 나갔을 때
        {
            isOnPortal = false;
        }
    }

    void ApplyMaterialToPortal()
    {
        // 현재 오브젝트의 Renderer를 가져옵니다.
        Renderer portalRenderer = GetComponent<Renderer>();

        if (portalRenderer != null)
        {
            // Renderer의 Material을 새로운 Material로 변경합니다.
            portalRenderer.material = material;
            //Debug.Log("New material applied to the portal.");
        }
    }

    IEnumerator NextScene()
    {
        player.GetComponent<PlayerController>().enabled = false;
        playerAnimator.SetBool("isWalking", false);
        transform.GetComponent<Animator>().SetBool("isTeleport", true);
        player.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
        playerGun.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        greyBG_up.GetComponent<RectTransform>().DOAnchorPosY(270f, 0.25f).SetEase(Ease.InSine);
        greyBG_down.GetComponent<RectTransform>().DOAnchorPosY(-270f, 0.25f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.3f);
        if (isMiddleBoss)
            SceneLoader.LoadSceneWithLoading("scene3_soon");
        else
            SceneLoader.LoadSceneWithLoading("spiderBossMap");
    }

    IEnumerator GoToBoss()
    {
        player.GetComponent<PlayerController>().enabled = false;
        playerAnimator.SetBool("isWalking", false);
        player.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
        playerGun.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        greyBG_up.GetComponent<RectTransform>().DOAnchorPosY(270f, 0.25f).SetEase(Ease.InSine);
        greyBG_down.GetComponent<RectTransform>().DOAnchorPosY(-270f, 0.25f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.3f);
        SceneLoader.LoadSceneWithLoading("boss_soon");
    }
}
