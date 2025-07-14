using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
        public Transform target; // ���� ���
        public float smoothSpeed = 0.125f; // �ε巯�� �̵� �ӵ�
    public Vector3 offset; // ī�޶�� ��� ������ ������

    private void LateUpdate()
    {
        if (target != null)
        {
            // ��ǥ ��ġ = ĳ���� ��ġ + ������ �Ÿ�
            Vector3 desiredPosition = target.position + offset;
            // ���� ��ġ���� ��ǥ ��ġ�� �ε巴�� �̵�
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // ī�޶� �׻� ����� ������ �ٶ󺸵��� ����
            transform.LookAt(target);
        }
    }
}
