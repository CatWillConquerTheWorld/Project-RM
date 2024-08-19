using UnityEngine;

public class Sweeper : MonoBehaviour
{
    private EnemyController enemyController;

    private GameObject attack1Collider;
    private GameObject attack2Collider;
    private GameObject longAttackCollider;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        attack1Collider = transform.Find("attack1Collider").gameObject;
        attack1Collider.SetActive(false);
        attack2Collider = transform.Find("attack2Collider").gameObject;
        attack2Collider.SetActive(false);
        longAttackCollider = transform.Find("longAttackCollider").gameObject;
        longAttackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Attack()
    {
        attack1Collider.SetActive(false);
        attack2Collider.SetActive(false);
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

        if (enemyController.attackStack % 2 == 1) 
        { 
            enemyController.animator.SetTrigger("attack1"); 
            attack1Collider.SetActive(true); 
            attack2Collider.SetActive(false); 
        }
        if (enemyController.attackStack % 2 == 0) 
        { 
            enemyController.animator.SetTrigger("attack2"); 
            attack2Collider.SetActive(true); 
            attack1Collider.SetActive(false); 
        }
    }

    public void LongAttackPrepare()
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

    public void LongAttack()
    {
        longAttackCollider.SetActive(true);
        enemyController.animator.SetTrigger("longAttack");
    }

    public void attackStackPlus()
    {
        enemyController.attackStack += 1;
    }

    public void back()
    {
        enemyController.isCharging = false;
        enemyController.animator.SetBool("isCharging", false);
        attack1Collider.SetActive(false);
        attack2Collider.SetActive(false);
        longAttackCollider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }
}
