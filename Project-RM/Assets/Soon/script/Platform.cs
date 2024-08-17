using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    public GameObject[] platforms; // ���� 3���� ���� �迭
    public float toggleInterval = 2f; // ������ ������� ��Ÿ���� ���� (��)

    private int currentPlatformIndex = 0; // ���� Ȱ��ȭ�� ������ �ε���

    void Start()
    {
        // ��� ������ ��Ȱ��ȭ�ϰ�, ù ��° ���Ǹ� Ȱ��ȭ
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false);
        }
        platforms[currentPlatformIndex].SetActive(true);

        // ������ Ȱ��ȭ/��Ȱ��ȭ�� �ݺ��ϴ� �ڷ�ƾ ����
        StartCoroutine(TogglePlatforms());
    }

    IEnumerator TogglePlatforms()
    {
        while (true) // ���� �ݺ�
        {
            // ���� Ȱ��ȭ�� ������ ��Ȱ��ȭ
            platforms[currentPlatformIndex].GetComponent<Material>().DOFade(0f, 0.1f).SetEase(Ease.OutSine).OnComplete(() => platforms[currentPlatformIndex].gameObject.SetActive(false));

            // ���� ������ �ε����� ����
            currentPlatformIndex = (currentPlatformIndex + 1) % platforms.Length;

            // ���� ������ Ȱ��ȭ
            platforms[currentPlatformIndex].GetComponent<Material>().DOFade(1f, 0.1f).SetEase(Ease.OutSine).OnStart(() => platforms[currentPlatformIndex].gameObject.SetActive(true));

            // ������ �ð���ŭ ���
            yield return new WaitForSeconds(toggleInterval);
        }
    }
}
