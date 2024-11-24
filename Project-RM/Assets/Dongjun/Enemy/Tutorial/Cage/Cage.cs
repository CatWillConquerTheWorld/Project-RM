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
    }


    public void Attack()
    {
        attack1Collider.SetActive(false);
        enemyController.velocity = 0;
        enemyController.animator.SetTrigger("attack1");
        attack1Collider.SetActive(true);
    }

    public void back()
    {
        enemyController.isCharging = false;
        attack1Collider.SetActive(false);
        enemyController.animator.SetTrigger("back");
    }

    public void Dead()
    {
        if (Tutorial.Instance != null) { 
            Tutorial.Instance.deadEnemies += 1; 
        }
        gameObject.SetActive(false);
    }
}
