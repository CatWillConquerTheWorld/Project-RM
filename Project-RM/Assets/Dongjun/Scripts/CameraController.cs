using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Vector3 offset; // 플레이어로부터의 오프셋 거리
    public float smoothSpeed = 0.75f; // 부드러운 이동을 위한 속도

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
        // 현재 위치와 목표 위치 사이의 부드러운 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치를 부드럽게 이동
        transform.position = smoothedPosition;
    }
}
