using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    public GameObject spawnPoints;
    public int spawnAmount;
    private GameObject[] spawnPointList;

    public GameObject[] enemiesToSpawn;
    public int[] amountOfEnemies;

    public static List<GameObject> enemies = new List<GameObject>();

    public float stageBPM;

    public TMP_Text readyText;
    public TMP_Text countText;
    private float readyTextCharacterSpace;

    // Start is called before the first frame update
    void Start()
    {
        spawnPointList = new GameObject[spawnAmount];
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
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
        yield return new WaitForSeconds(4f);
        int spawnIndex = 0;
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
        countText.text = "1";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.text = "GO!";
        yield return new WaitForSeconds(60f / stageBPM);
        countText.enabled = false;
        yield return StartCoroutine(WaitForElemenation());
        print("Well done!");
        readyText.text = "Clear!";
        DOTween.To(() => readyTextCharacterSpace, x => readyTextCharacterSpace = x, 50f, 2f).SetEase(Ease.OutSine).OnUpdate(() => readyText.characterSpacing = readyTextCharacterSpace).OnStart(() => readyText.enabled = true);
        readyText.DOFade(1f, 2f).SetEase(Ease.InQuart);
    }

    IEnumerator WaitForElemenation()
    {
        while (true)
        {
            if (player.GetComponent<PlayerController>().GetIsDead())
            {
                print("GameOver");
                yield break;
            }
            else if (enemies.Count == 0)
            {
                print("Clear!");
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
