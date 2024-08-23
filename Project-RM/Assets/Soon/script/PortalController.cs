using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public Animator portalAnimator;
    private bool isOnPortal = false; // 플레이어가 포탈 A 위에 있는지 여부
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
        if (other.CompareTag("Player")) // 플레이어가 포탈에 들어왔을 때
        {
            isOnPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 포탈에서 나갔을 때
        {
            isOnPortal = false;
        }
    }

    void ApplyMaterialToPortal()
    {
        // 현재 오브젝트의 Renderer를 가져옵니다.
        Renderer portalRenderer = GetComponent<Renderer>();

        if (portalRenderer != null)
        {
            // Renderer의 Material을 새로운 Material로 변경합니다.
            portalRenderer.material = material;
            Debug.Log("New material applied to the portal.");
        }
    }
}
