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

    private Rigidbody rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Update에서는 키보드 입력과 같이 매 프레임 확인해야 하는 것들을 처리합니다.
        // 이동 방향을 계산하고, 애니메이션 파라미터를 설정하는 것까지는 여기서 합니다.
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        anim.SetBool("isWalking", moveDirection != Vector3.zero);

        // 가드 키 입력 확인
        HandleGuardInput();

        // 상태에 따른 행동 시작 (물리 효과와 관련 없는 부분)
        if (isDodging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDodging = false;
                isAttacking = true;
                anim.SetTrigger("Attack");
            }
            return;
        }

        if (isAttacking || isCharging || isGuarding)
        {
            return;
        }

        HandleActionInitiation();
    }

    void FixedUpdate()
    {
        // FixedUpdate에서는 물리 효과와 관련된 것을 처리합니다.
        // 실제 캐릭터를 움직이는 것은 여기서 처리해야 합니다.
        HandleMovement();
    }

    void HandleGuardInput()
    {
        isGuarding = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("isGuarding", isGuarding);
    }

    void HandleMovement()
    {
        float currentMoveSpeed = isGuarding ? moveSpeed * guardMoveSpeedFactor : moveSpeed;

        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * currentMoveSpeed * Time.fixedDeltaTime); // Time.deltaTime 대신 Time.fixedDeltaTime 사용
            rb.MoveRotation(Quaternion.LookRotation(moveDirection)); // transform.rotation 대신 rb.MoveRotation 사용
        }
    }

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