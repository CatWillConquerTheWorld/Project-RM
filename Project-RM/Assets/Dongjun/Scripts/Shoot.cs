using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ��ġ�� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // z�� ���� 0���� �����Ͽ� 2D ��鿡���� ��ġ�� ���
        mousePosition.z = 0;

        // ��ü�� ���� ��ġ
        Vector3 objectPosition = transform.position;

        // ��ü�� ���콺 ��ġ ���� ���� ���� ���
        Vector2 direction = mousePosition - objectPosition;

        // ���� ������ ������ ����Ͽ� ��ü�� ȸ�������� ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��ü�� ȸ���� ���� (z���� �������� ȸ��)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
