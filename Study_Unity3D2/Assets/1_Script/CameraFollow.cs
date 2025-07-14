using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
        public Transform target; // 따라갈 대상
        public float smoothSpeed = 0.125f; // 부드러운 이동 속도
    public Vector3 offset; // 카메라와 대상 사이의 오프셋

    private void LateUpdate()
    {
        if (target != null)
        {
            // 목표 위치 = 캐릭터 위치 + 정해진 거리
            Vector3 desiredPosition = target.position + offset;
            // 현재 위치에서 목표 위치로 부드럽게 이동
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 카메라가 항상 대상의 방향을 바라보도록 설정
            transform.LookAt(target);
        }
    }
}
