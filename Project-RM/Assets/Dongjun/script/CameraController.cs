using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    public Transform player; // �÷��̾��� Transform
    public Vector3 offset; // �÷��̾�κ����� ������ �Ÿ�
    public float smoothSpeed = 0.75f; // �ε巯�� �̵��� ���� �ӵ�

    void Start()
    {
        offset = Vector3.zero;
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    void LateUpdate()
    {
        // ��ǥ ��ġ ���� (z���� ����)
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        // ���� ��ġ�� ��ǥ ��ġ ������ �ε巯�� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ī�޶� ��ġ�� �ε巴�� �̵�
        transform.position = smoothedPosition;
    }
}
