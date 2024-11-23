using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Entrance : MonoBehaviour
{
    public Animator animator;
    public GameObject keyF;

    public GameObject player;
    private GameObject playerGun;
    private Animator playerAnimator;
    public GameObject greyBG_up;
    public GameObject greyBG_down;

    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();

        playerGun = player.transform.Find("Gun").gameObject;
        keyF.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            animator.SetBool("lightUp", true);
            keyF.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                StartCoroutine(NextScene());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("lightUp", false);
            keyF.SetActive(false);
        }
    }

    IEnumerator NextScene()
    {
        player.GetComponent<PlayerController>().enabled = false;
        yield return StartCoroutine(PlayerMoveX(51.90f, 3f));
        playerAnimator.SetBool("isWalking", true);
        player.GetComponent<SpriteRenderer>().DOFade(0f, 1f);
        playerGun.GetComponent<SpriteRenderer>().DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
        greyBG_up.GetComponent<RectTransform>().DOAnchorPosY(270f, 0.25f).SetEase(Ease.InSine);
        greyBG_down.GetComponent<RectTransform>().DOAnchorPosY(-270f, 0.25f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(0.3f);
        SceneLoader.LoadSceneWithLoading("Stage1");
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
        //playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetFloat("SpeedHandler", 1f);
        yield return null;
    }
}
