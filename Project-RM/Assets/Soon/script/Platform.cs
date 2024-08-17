using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    public GameObject[] platforms; // 발판 3개를 담을 배열
    public float toggleInterval = 2f; // 발판이 사라졌다 나타나는 간격 (초)

    private int currentPlatformIndex = 0; // 현재 활성화된 발판의 인덱스

    void Start()
    {
        // 모든 발판을 비활성화하고, 첫 번째 발판만 활성화
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false);
        }
        platforms[currentPlatformIndex].SetActive(true);

        // 발판의 활성화/비활성화를 반복하는 코루틴 시작
        StartCoroutine(TogglePlatforms());
    }

    IEnumerator TogglePlatforms()
    {
        while (true) // 무한 반복
        {
            // 현재 활성화된 발판을 비활성화
            platforms[currentPlatformIndex].GetComponent<Material>().DOFade(0f, 0.1f).SetEase(Ease.OutSine).OnComplete(() => platforms[currentPlatformIndex].gameObject.SetActive(false));

            // 다음 발판의 인덱스로 변경
            currentPlatformIndex = (currentPlatformIndex + 1) % platforms.Length;

            // 다음 발판을 활성화
            platforms[currentPlatformIndex].GetComponent<Material>().DOFade(1f, 0.1f).SetEase(Ease.OutSine).OnStart(() => platforms[currentPlatformIndex].gameObject.SetActive(true));

            // 지정된 시간만큼 대기
            yield return new WaitForSeconds(toggleInterval);
        }
    }
}
