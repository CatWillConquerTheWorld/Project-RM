using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    public GameObject[] platforms; // �����е� �迭
    public float toggleInterval = 2f; // �����е��� Ȱ��ȭ ���� (��)
    public float fadeDuration = 0.5f; // ���̵� ȿ�� ���� �ð�

    private int currentPlatformIndex = 0; // ���� Ȱ��ȭ�� �����е��� �ε���

    void Start()
    {
        // ��� �����е��� �ڽ� ������Ʈ�� �����ϰ� �ʱ�ȭ
        foreach (GameObject platform in platforms)
        {
            foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
            {
                Color color = sprite.color;
                color.a = 0f; // ������ �����ϰ� ����
                sprite.color = color;
            }
            platform.SetActive(false); // ��Ȱ��ȭ
        }

        // ù ��° �����е� Ȱ��ȭ �� ���̵� ��
        platforms[currentPlatformIndex].SetActive(true);
        FadeInChildren(platforms[currentPlatformIndex]);

        // �����е��� Ȱ��ȭ/��Ȱ��ȭ�� �ݺ��ϴ� �ڷ�ƾ ����
        StartCoroutine(TogglePlatforms());
    }

    IEnumerator TogglePlatforms()
    {
        while (true) // ���� �ݺ�
        {
            // ���� Ȱ��ȭ�� �����е� ���̵� �ƿ�
            FadeOutChildren(platforms[currentPlatformIndex]);
            yield return new WaitForSeconds(fadeDuration); // ���̵� �ƿ��� ���� ������ ���
            platforms[currentPlatformIndex].SetActive(false);

            // ���� �����е��� �ε����� ����
            currentPlatformIndex = (currentPlatformIndex + 1) % platforms.Length;

            // ���� �����е� Ȱ��ȭ �� ���̵� ��
            platforms[currentPlatformIndex].SetActive(true);
            FadeInChildren(platforms[currentPlatformIndex]);

            // ������ �ð���ŭ ���
            yield return new WaitForSeconds(toggleInterval);
        }
    }

    private void FadeInChildren(GameObject platform)
    {
        // �ڽĵ��� SpriteRenderer ������ ���̵� ��
        foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite != null) // SpriteRenderer�� �ִ� ��쿡�� ó��
            {
                sprite.DOFade(1f, fadeDuration).SetEase(Ease.OutSine);
            }
        }
    }

    private void FadeOutChildren(GameObject platform)
    {
        // �ڽĵ��� SpriteRenderer ������ ���̵� �ƿ�
        foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite != null)
            {
                sprite.DOFade(0f, fadeDuration).SetEase(Ease.InSine); // ���� ���� 0���� ����
            }
        }
    }
}
