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
    private Stage1 stage1;

    private void Start()
    {
        stage1 = GetComponent<Stage1>();
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

        if (isOnPortal && isNoEnemy)
        {
            KeyF.SetActive(true);
            
            if(Input.GetKeyDown(KeyCode.F))
            {
                SceneLoader.LoadSceneWithLoading("boss_soon");
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
}
