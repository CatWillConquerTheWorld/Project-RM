using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warden : MonoBehaviour
{
    private EnemyController enemyController;

    private GameObject attackCollider;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        attackCollider = transform.Find("attackCollider").gameObject;
        attackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Attack()
    {
        attackCollider.SetActive(false);
        enemyController.velocity = 0;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction.x < 0f)
        {

            transform.localScale = new Vector3(-3, 3, 0);
        }
        else
        {
            transform.localScale = new Vector3(3, 3, 0);
        }
        attackCollider.SetActive(true);
        enemyController.animator.SetTrigger("attack");
    }

    public void back()
    {
        attackCollider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }
}
