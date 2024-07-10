using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    private EnemyController enemyController;
    private GameObject attackCollider;
    private GameObject longAttackCollider;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        attackCollider = transform.Find("attackCollider").gameObject;
        attackCollider.SetActive(false);
        longAttackCollider = transform.Find("longAttackCollider").gameObject;
        longAttackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            LongAttackPrepare();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            LongAttack();
        }
    }

    void Attack()
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

    void LongAttackPrepare()
    {
        longAttackCollider.SetActive(false);
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
        enemyController.isCharging = true;
        enemyController.animator.SetBool("isCharging", true);
    }

    void LongAttack()
    {
        longAttackCollider.SetActive(true);
        enemyController.animator.SetTrigger("longAttack");
    }

    public void back()
    {
        enemyController.isCharging = false;
        enemyController.animator.SetBool("isCharging", false);
        attackCollider.SetActive(false);
        longAttackCollider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }

    public IEnumerator ChainHit()
    {
        longAttackCollider.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        longAttackCollider.SetActive(false);
        yield return new WaitForSeconds(0.025f);
        longAttackCollider.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        longAttackCollider.SetActive(false);
        yield return new WaitForSeconds(0.025f);
        longAttackCollider.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        longAttackCollider.SetActive(false);
        yield return new WaitForSeconds(0.025f);
    }

    public void DisableAttackCollider()
    {
        attackCollider.SetActive(false);
    }
}
