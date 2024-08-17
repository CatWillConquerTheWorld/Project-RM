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
        // 마우스 위치를 화면 좌표에서 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // z축 값을 0으로 설정하여 2D 평면에서의 위치만 사용
        mousePosition.z = 0;

        // 객체의 현재 위치
        Vector3 objectPosition = transform.position;

        // 객체와 마우스 위치 간의 방향 벡터 계산
        Vector2 direction = mousePosition - objectPosition;

        // 방향 벡터의 각도를 계산하여 객체의 회전값으로 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 객체의 회전을 설정 (z축을 기준으로 회전)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
