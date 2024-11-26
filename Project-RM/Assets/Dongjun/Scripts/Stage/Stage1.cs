using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerPlayerController;

    public GameObject spawnPoints;
    public int spawnAmount;
    private GameObject[] spawnPointList;

    public GameObject[] enemiesToSpawn;
    public int[] amountOfEnemies;

    public static List<GameObject> enemies = new List<GameObject>();
    public static bool isSpawn = false;

    public float stageBPM;

    public TMP_Text readyText;
    public TMP_Text countText;
    private float readyTextCharacterSpace;

    public RectTransform greyBG_up;
    public RectTransform greyBG_down;

    public bool isOkToGo = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnPointList = new GameObject[spawnAmount];
        player = GameObject.FindWithTag("Player");
        playerPlayerController = player.GetComponent<PlayerController>();
        for (int i = 0; i < spawnPoints.transform.childCount; i++)
        {
            spawnPointList[i] = spawnPoints.transform.GetChild(i).gameObject;
        }
        spawnPointList = ShuffleArray<GameObject>(spawnPointList);
        countText.enabled = false;
        readyTextCharacterSpace = 50f;
        StartCoroutine(StageFlow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StageFlow()
    {
        playerPlayerController.enabled = false;
        yield return new WaitForSeconds(0.5f);
        greyBG_up.DOAnchorPosY(810f, 0.3f).SetEase(Ease.InSine);
        greyBG_down.DOAnchorPosY(-810f, 0.3f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(1f);
        int spawnIndex = 0;
        enemies.Clear();
        for (int i = 0; i < amountOfEnemies.Length; i++)
        {
            for (int j = 0; j < amountOfEnemies[i]; j++)
            {
                GameObject enemy = Instantiate(enemiesToSpawn[i], spawnPointList[spawnIndex].transform.position, Quaternion.identity);
                enemy.SetActive(true);
                enemies.Add(enemy);
                spawnIndex++;   
            }
        }
        DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 300f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnComplete(() => readyText.enabled = false);
        readyText.DOFade(0f, 2f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(2f);
        countText.enabled = true;
        countText.text = "3";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.text = "2";
        yield return new WaitForSeconds(60f / stageBPM);
        LoadBMS.currentTime = 0d;
        LoadBMS.Instance.play_song("WTF");                      //≥Î∑° ¿Ã∏ß πŸ≤„æﬂ«‘
        countText.text = "1";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.text = "GO!";
        playerPlayerController.enabled = true;
        yield return new WaitForSeconds(60f / stageBPM);
        countText.enabled = false;
        yield return StartCoroutine(WaitForElemenation());
        //isSpawn = true;
        isOkToGo = true;
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
            else if (enemies.Count == 0)
            {
                readyText.text = "∆˜≈ª¿Ã ø≠∑»Ω¿¥œ¥Ÿ.";
                DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 50f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnStart(() => readyText.enabled = true);
                readyText.DOFade(1f, 2f).SetEase(Ease.InQuart);
                CenterFrame.MusicFadeOut();
                yield break;
            }
            yield return null;
        }
    }

    private T[] ShuffleArray<T>(T[] array)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }
}
