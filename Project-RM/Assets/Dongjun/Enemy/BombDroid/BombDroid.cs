using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDroid : MonoBehaviour
{
    private EnemyController enemyController;

    private GameObject attackBeam;
    private Transform attackBeamPoint;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        attackBeam = transform.Find("attackBeam").gameObject;
        attackBeamPoint = transform.Find("attackBeamPoint");
        attackBeam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Attack()
    {
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
        enemyController.animator.SetTrigger("attack");
    }

    void attackBeamActivate()
    {
        GameObject bomb = Instantiate(attackBeam,attackBeamPoint);
        bomb.GetComponent<BoxCollider2D>().enabled = false;
        bomb.SetActive(true);
    }

    public void back()
    {
        enemyController.animator.SetTrigger("back");
    }
}
