using UnityEngine;

public class Cage : MonoBehaviour
{
    private EnemyController enemyController;

    private GameObject attack1Collider;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        attack1Collider = transform.Find("attackCollider").gameObject;
        attack1Collider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Attack();
        } 
    }


    void Attack()
    {
        attack1Collider.SetActive(false);
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
        enemyController.animator.SetTrigger("attack1");
        attack1Collider.SetActive(true);
    }

    public void back()
    {
        enemyController.isCharging = false;
        enemyController.animator.SetBool("isCharging", false);
        attack1Collider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }
}
