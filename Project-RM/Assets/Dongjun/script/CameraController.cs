using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public Vector3 offset; // �÷��̾�κ����� ������ �Ÿ�
    public float smoothSpeed = 0.75f; // �ε巯�� �̵��� ���� �ӵ�

    public bool movingYEnabled;
    private float movingY;

    private Vector3 cameraPosition;

    void Start()
    {
        CameraMove(true);
    }

    void LateUpdate()
    {
        CameraMove(movingYEnabled);
    }

    void CameraMove(bool isMoveToYEnabled)
    {
        if (isMoveToYEnabled)
        {
            movingY = player.position.y;
        }
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, movingY + offset.y, transform.position.z);
        // ���� ��ġ�� ��ǥ ��ġ ������ �ε巯�� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ī�޶� ��ġ�� �ε巴�� �̵�
        transform.position = smoothedPosition;
    }
}
