using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wave
{
    public int numberOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}
public class WaveStart : MonoBehaviour
{
    public Wave wave;
    public Transform[] spawnPoint;

    public GameObject player;
    private GameObject playerGun;
    private Animator playerAnimator;
    public GameObject keyP;
    private bool isNext;
    private bool isSpawn;
    private bool canSpawn = true;
    
    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public float targetXPosition;
    public float statueXPosition; // 석상의 x 위치
    public float moveSpeed = 2.0f;
    private float nextSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();
        playerGun = player.transform.Find("Gun").gameObject;
        isNext = false;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(SpawnWave());
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && !canSpawn)
        {
            Clear();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isNext = true;
            isSpawn = true;
        }

        if (Mathf.Abs(player.transform.position.x - targetXPosition) < 0.1f)
        {
            StartCoroutine(TriggerEvent());
        }

        if (isSpawn)
        {
            StartCoroutine(BeforeWave());
            isSpawn = false;
        }
    }

    public void Clear()
    {
        Debug.Log("Wave Clear!");
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = wave.typeOfEnemies[Random.Range(0, wave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoint[Random.Range(0, spawnPoint.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            wave.numberOfEnemies--;
            nextSpawnTime = Time.time + wave.spawnInterval;
            if (wave.numberOfEnemies == 0)
            {
                canSpawn = false;
            }
        }
    }

    IEnumerator BeforeWave()
    {
        yield return StartCoroutine(CameraMoveY(1f, 1f, "flex"));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(CameraShake(5f, 0.1f, 40, false));
        yield return StartCoroutine(CameraShake(1f, 0.1f, 40, true));
        CameraReturns();
        SpawnWave();
        //yield return StartCoroutine(SpawnWave());

    }
    IEnumerator TriggerEvent()
    {
        
        // 상하단 바 생성
        yield return StartCoroutine(MovieStart());
        yield return new WaitForSeconds(1.0f);
        // 플레이어를 석상의 앞으로 이동
        yield return StartCoroutine(PlayerMoveX(statueXPosition, moveSpeed));
        yield return WaitForUser();
        // 이후에 추가적으로 수행할 작업이 있다면 여기에 작성
    }
    IEnumerator WaitForUser()
    {
        isNext = false;
        keyP.SetActive(true);
        while (!isNext) yield return null;
        keyP.SetActive(false);
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

    IEnumerator CameraMoveY(float amount, float duration, string method)
    {
        if (method == "flex") amount += Camera.main.transform.position.y;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        yield return Camera.main.transform.DOMoveY(amount, duration).SetEase(Ease.Linear).WaitForCompletion();
        //Camera.main.transform.DOMoveY(amount, duration).SetEase(Ease.Linear);
        //yield return new WaitForSeconds(duration);

    }
}
