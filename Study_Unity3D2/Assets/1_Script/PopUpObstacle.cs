using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpObstacle : MonoBehaviour
{
    [Tooltip("튀어나올 최대 높이")]
    public float popUpheight = 0.5f;

    [Tooltip("튀어나오는 데 걸리는 시간")]
    public float popUpDuration = 2.0f;

    [Tooltip("튀어나온 후 유지하는 시간")]
    public float stayUpDuration = 1.0f;

    [Tooltip("다시 튀어나오기까지 대기 시간")]
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
        // 시작 시 약간의 랜덤 시간차 주기
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
                    timer = 0f; // 타이머 초기화
                }
                break;
            case State.PoppingUp:
                float popUpProgress = Mathf.Clamp01(timer / popUpDuration);
                transform.position = Vector3.Lerp(initialPosition, targetPosition, popUpProgress);
                if (popUpProgress >= 1f)
                {
                    currentState = State.StayingUp;
                    timer = 0f; // 타이머 초기화
                }
                break;
            case State.StayingUp:
                if (timer >= stayUpDuration)
                {
                    currentState = State.CoolingDown;
                    timer = 0f; // 타이머 초기화
                }
                break;
            case State.CoolingDown:
                float coolDownProgress = Mathf.Clamp01(timer / popUpDuration);
                transform.position = Vector3.Lerp(targetPosition, initialPosition, coolDownProgress);
                if (coolDownProgress >= 1f)
                {
                    currentState = State.Idle;
                    timer = 0f; // 타이머 초기화
                }
                break;
        }
    }
}
