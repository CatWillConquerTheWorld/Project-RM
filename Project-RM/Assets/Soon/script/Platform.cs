using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    public GameObject[] platforms; // 점프패드 배열
    public float toggleInterval = 2f; // 점프패드의 활성화 간격 (초)
    public float fadeDuration = 0.5f; // 페이드 효과 지속 시간

    private int currentPlatformIndex = 0; // 현재 활성화된 점프패드의 인덱스

    void Start()
    {
        // 모든 점프패드의 자식 오브젝트를 투명하게 초기화
        foreach (GameObject platform in platforms)
        {
            foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
            {
                Color color = sprite.color;
                color.a = 0f; // 완전히 투명하게 설정
                sprite.color = color;
            }
            platform.SetActive(false); // 비활성화
        }

        // 첫 번째 점프패드 활성화 및 페이드 인
        platforms[currentPlatformIndex].SetActive(true);
        FadeInChildren(platforms[currentPlatformIndex]);

        // 점프패드의 활성화/비활성화를 반복하는 코루틴 시작
        StartCoroutine(TogglePlatforms());
    }

    IEnumerator TogglePlatforms()
    {
        while (true) // 무한 반복
        {
            // 현재 활성화된 점프패드 페이드 아웃
            FadeOutChildren(platforms[currentPlatformIndex]);
            yield return new WaitForSeconds(fadeDuration); // 페이드 아웃이 끝날 때까지 대기
            platforms[currentPlatformIndex].SetActive(false);

            // 다음 점프패드의 인덱스로 변경
            currentPlatformIndex = (currentPlatformIndex + 1) % platforms.Length;

            // 다음 점프패드 활성화 및 페이드 인
            platforms[currentPlatformIndex].SetActive(true);
            FadeInChildren(platforms[currentPlatformIndex]);

            // 지정된 시간만큼 대기
            yield return new WaitForSeconds(toggleInterval);
        }
    }

    private void FadeInChildren(GameObject platform)
    {
        // 자식들의 SpriteRenderer 투명도를 페이드 인
        foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite != null) // SpriteRenderer가 있는 경우에만 처리
            {
                sprite.DOFade(1f, fadeDuration).SetEase(Ease.OutSine);
            }
        }
    }

    private void FadeOutChildren(GameObject platform)
    {
        // 자식들의 SpriteRenderer 투명도를 페이드 아웃
        foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite != null)
            {
                sprite.DOFade(0f, fadeDuration).SetEase(Ease.InSine); // 알파 값을 0으로 설정
            }
        }
    }
}
