using UnityEngine;

public class OrbMage : MonoBehaviour
{
    private EnemyController enemyController;

    private Transform attackBeam;
    private GameObject longAttackCollider;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        attackBeam = transform.Find("attackBeam");
        attackBeam.gameObject.SetActive(false);
        longAttackCollider = transform.Find("longAttackCollider").gameObject;
        longAttackCollider.SetActive(false);
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
        if (PlayerCheck())
        {
            RaycastHit2D hit = Physics2D.Raycast(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>().bounds.center, Vector2.down, 10, LayerMask.GetMask("Ground"));

            // 땅이 감지되었을 때
            if (hit.collider != null)
            {
                GameObject attackBeamObj = Instantiate(attackBeam.gameObject, hit.point, Quaternion.identity);
                attackBeamObj.SetActive(true);
                attackBeamObj.GetComponent<BoxCollider2D>().enabled = true;
                attackBeamObj.GetComponent<Animator>().SetTrigger("attackBeam");
            }

        }
    }

    bool PlayerCheck()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= enemyController.playerRadius)
        {
            return true;
        }
        else
        {
            return false;
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

    public void back()
    {
        enemyController.isCharging = false;
        enemyController.animator.SetBool("isCharging", false);
        longAttackCollider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }
}
