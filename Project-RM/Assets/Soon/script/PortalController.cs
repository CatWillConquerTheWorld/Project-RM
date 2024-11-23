using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public Animator portalAnimator;
    private bool isOnPortal = false; // �÷��̾ ��Ż A ���� �ִ��� ����
    private bool isNoEnemy = false;
    public Material material;
    public GameObject KeyF;
    public GameObject[] enemies;
    public Stage1 stage1;

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
            
            if(Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(NextScene());
            }
        }
        else KeyF.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ��Ż�� ������ ��
        {
            isOnPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ��Ż���� ������ ��
        {
            isOnPortal = false;
        }
    }

    void ApplyMaterialToPortal()
    {
        // ���� ������Ʈ�� Renderer�� �����ɴϴ�.
        Renderer portalRenderer = GetComponent<Renderer>();

        if (portalRenderer != null)
        {
            // Renderer�� Material�� ���ο� Material�� �����մϴ�.
            portalRenderer.material = material;
            Debug.Log("New material applied to the portal.");
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
        SceneLoader.LoadSceneWithLoading("boss_soon");
    }
}
