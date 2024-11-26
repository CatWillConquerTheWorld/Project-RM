using UnityEngine;

public class Assasin : MonoBehaviour
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
        if (enemyController.attackStack % 2 == 1) enemyController.animator.SetTrigger("attack1");
        if (enemyController.attackStack % 2 == 0) enemyController.animator.SetTrigger("attack2");
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
        enemyController.animator.SetBool("isCharging", false);
    }

    public void attackStackPlus()
    {
        enemyController.attackStack += 1;
    }

    public void back()
    {
        enemyController.isCharging = false;
        //enemyController.animator.SetBool("isCharging", false);
        attackCollider.SetActive(false);
        longAttackCollider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }
}
