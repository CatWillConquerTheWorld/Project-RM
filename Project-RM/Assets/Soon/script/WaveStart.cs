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
    public GameObject monsterSpawn;
    private Animator playerAnimator;
    private PlayerController playerPlayerController;
    public GameObject keyP;
    public GameObject door;
    private Animator doorAnimator;
    private bool isNext;
    public bool isSpawn;
    public bool isClear = false;
    private bool isCinematic = false;
    private bool canSpawn = true;
    
    public RectTransform movieEffectorUp;
    public RectTransform movieEffectorDown;

    public float targetXPosition;
    public float statueXPosition; // 석상의 x 위치
    public float moveSpeed = 2.0f;
    private float nextSpawnTime;
    // Start is called before the first frame update

    public AudioSource quake;
    void Start()
    {
        playerPlayerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        doorAnimator = door.GetComponent<Animator>();
        playerGun = player.transform.Find("Gun").gameObject;
        isNext = false;
        isSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(SpawnWave());
        //GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Stage2.enemies.Count == 0 && !canSpawn && !isClear)
        {
            
            StartCoroutine(Clear());
            isClear = true;
        }
        
        if (!isCinematic)
        {
            if (Mathf.Abs(player.transform.position.x - targetXPosition) < 0.1f)
            {
                StartCoroutine(TriggerEvent());
                isCinematic = true;
            }
        }
        

        if (isSpawn)
        {
            StartCoroutine(WaitForUser());
        }
    }

    IEnumerator Clear()
    {
        Stage2.waveClear = true;
        playerPlayerController.enabled = false;
        yield return StartCoroutine(MovieStart());
        StartCoroutine(CameraMoveX(56.3f, 1f, "noflex"));
        StartCoroutine(CameraMoveY(-37.2f, 1f, "noflex"));
        yield return new WaitForSeconds(1f);
        doorAnimator.SetBool("isOpen", true);
        door.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(3f);
        CameraReturns();
        playerPlayerController.enabled = true;
        yield return StartCoroutine(MovieEnd());

        
    }

    IEnumerator SpawnWave()
    {
        while (canSpawn && wave.numberOfEnemies > 0) // 적이 남아있을 때까지 반복
        {
            if (nextSpawnTime < Time.time)
            {
                GameObject randomEnemy = wave.typeOfEnemies[Random.Range(0, wave.typeOfEnemies.Length)];
                Transform randomPoint = spawnPoint[Random.Range(0, spawnPoint.Length)];

                GameObject spawnedEnemy = Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
                // 적 생성
                spawnedEnemy.SetActive(true);
                Stage2.enemies.Add(spawnedEnemy);
                // 적의 수를 감소
                wave.numberOfEnemies--;

                // 다음 스폰 시간을 설정
                nextSpawnTime = Time.time + wave.spawnInterval;

                if (wave.numberOfEnemies == 0)
                {
                    canSpawn = false;
                }
            }

            // 스폰 인터벌 동안 대기
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    IEnumerator BeforeWave()
    {
        yield return StartCoroutine(CameraMoveY(3.5f, 1f, "flex"));
        yield return new WaitForSeconds(1f);
        quake.Play();
        yield return StartCoroutine(CameraShake(5f, 0.1f, 40, false));
        monsterSpawn.SetActive(true);
        yield return StartCoroutine(CameraShake(1f, 0.1f, 40, true));
        CameraReturns();
        quake.Stop();
        Stage2.wave = true;
        StartCoroutine(MovieEnd());
        playerPlayerController.enabled = true;
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(SpawnWave());
    }

    IEnumerator TriggerEvent()
    {

        // 상하단 바 생성
        playerPlayerController.enabled = false;
        yield return StartCoroutine(MovieStart());
        yield return new WaitForSeconds(1.0f);
        // 플레이어를 석상의 앞으로 이동
        yield return StartCoroutine(PlayerMoveX(statueXPosition, moveSpeed));
        isSpawn = true;

        // 이후에 추가적으로 수행할 작업이 있다면 여기에 작성
    }
    public IEnumerator WaitForUser()
    {
        isNext = false;
        keyP.SetActive(true); // KeyP 활성화
        while (!isNext)
        {
            // P키를 눌렀는지 체크
            if (Input.GetKeyDown(KeyCode.P))
            {
                isNext = true; // 다음 단계로 넘어갈 수 있게 설정
                keyP.SetActive(false); // KeyP 비활성화
                StartCoroutine(BeforeWave()); // BeforeWave 실행
                isSpawn = false; // 스폰 상태 해제
            }

            yield return null; // 한 프레임 대기
        }
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
    }

    IEnumerator CameraMoveX(float amount, float duration, string method)
    {
        if (method == "flex") amount += Camera.main.transform.position.x;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        Camera.main.transform.DOMoveX(amount, duration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(duration);
    }
}
