using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpObstacle : MonoBehaviour
{
    [Tooltip("Ƣ��� �ִ� ����")]
    public float popUpheight = 0.5f;

    [Tooltip("Ƣ����� �� �ɸ��� �ð�")]
    public float popUpDuration = 2.0f;

    [Tooltip("Ƣ��� �� �����ϴ� �ð�")]
    public float stayUpDuration = 1.0f;

    [Tooltip("�ٽ� Ƣ�������� ��� �ð�")]
    public float cooldownDuration = 3.0f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float timer;
    private enum State { Idle, PoppingUp, StayingUp, CoolingDown }
    private State currentState = State.Idle;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * popUpheight;
        // ���� �� �ణ�� ���� �ð��� �ֱ�
        timer = Random.Range(0f, cooldownDuration);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState) 
        {
            case State.Idle:
                if (timer >= cooldownDuration)
                {
                    currentState = State.PoppingUp;
                    timer = 0f; // Ÿ�̸� �ʱ�ȭ
                }
                break;
            case State.PoppingUp:
                float popUpProgress = Mathf.Clamp01(timer / popUpDuration);
                transform.position = Vector3.Lerp(initialPosition, targetPosition, popUpProgress);
                if (popUpProgress >= 1f)
                {
                    currentState = State.StayingUp;
                    timer = 0f; // Ÿ�̸� �ʱ�ȭ
                }
                break;
            case State.StayingUp:
                if (timer >= stayUpDuration)
                {
                    currentState = State.CoolingDown;
                    timer = 0f; // Ÿ�̸� �ʱ�ȭ
                }
                break;
            case State.CoolingDown:
                float coolDownProgress = Mathf.Clamp01(timer / popUpDuration);
                transform.position = Vector3.Lerp(targetPosition, initialPosition, coolDownProgress);
                if (coolDownProgress >= 1f)
                {
                    currentState = State.Idle;
                    timer = 0f; // Ÿ�̸� �ʱ�ȭ
                }
                break;
        }
    }
}
