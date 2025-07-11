using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SantaController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;
    public float guardMoveSpeedFactor = 0.5f;
    public float requiredChargeDuration = 1f;

    private Animator anim;
    private Vector3 moveDirection;

    private bool isAttacking = false;
    private bool isDodging = false;
    private bool isCharging = false;
    private bool isGuarding = false;
    private float chargeTimer = 0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        // 1. 이동은 항상 처리 (공격 중에도 걷기 파라미터가 갱신되도록)
        HandleMovement();

        // 2. 가드 키 입력을 확인
        HandleGuardInput();

        // 3. 현재 상태에 따라 행동을 결정
        // 회피 중일 때는 공격으로 캔슬 가능
        if (isDodging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDodging = false;
                isAttacking = true;
                anim.SetTrigger("Attack");
            }
            return; // 회피 중에는 다른 행동 불가
        }

        // 공격, 차지, 가드 중일 때는 새로운 행동을 시작할 수 없음
        if (isAttacking || isCharging || isGuarding)
        {
            return;
        }

        // 모든 조건이 아닐 때만 새로운 행동(공격, 회피, 차지)을 시작
        HandleActionInitiation();
    }

    void HandleGuardInput()
    {
        isGuarding = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("isGuarding", isGuarding);
    }

    void HandleMovement()
    {
        // 가드 중이 아닐 때만 키 입력을 받아서 이동 방향을 갱신
        if (!isGuarding)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        }

        float currentMoveSpeed = isGuarding ? moveSpeed * guardMoveSpeedFactor : moveSpeed;

        anim.SetBool("isWalking", moveDirection != Vector3.zero);

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * currentMoveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    // 새로운 행동을 '시작'하는 것만 담당하는 함수
    void HandleActionInitiation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isDodging = true;
            anim.SetTrigger("Dodge");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (moveDirection != Vector3.zero) return;
            isCharging = true;
            chargeTimer = 0f;
            anim.SetBool("isCharging", true);
            anim.SetFloat("ChargeAmount", 0f);
        }
    }

    // 이 함수는 isCharging이 true일 때만 호출되어야 하므로, Update에서 직접 호출하는 대신
    // HandleActionInitiation 함수에서 isCharging = true;로 바꾼 후부터 자연스럽게
    // isCharging을 체크하는 로직이 필요. 기존 로직으로 회귀

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    public void OnDodgeEnd()
    {
        isDodging = false;
    }

}