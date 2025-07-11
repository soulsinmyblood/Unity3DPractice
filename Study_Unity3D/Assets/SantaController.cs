using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;
    public float requiredChargeDuration = 1f;

    private Animator anim;
    private Vector3 moveDirection;

    private bool isAttacking = false;
    private bool isDodging = false;
    private bool isCharging = false;
    private float chargeTimer = 0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking || isDodging)
        {
            return;
        }

        if (isCharging)
        {
            HandleCharging();
        }
        else
        {
            HandleMovement();
            HandleActions();
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        anim.SetBool("isWalking", moveDirection != Vector3.zero);

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    void HandleActions()
    {
        // 일반 공격 (마우스 좌클릭)
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
        // 회피 (스페이스바)
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isDodging = true;
            anim.SetTrigger("Dodge");
        }
        // 차지 시작 (마우스 우클릭 누르는 순간)
        else if (Input.GetMouseButtonDown(1))
        {
            // ▼▼▼ 이 부분이 추가되었습니다! ▼▼▼
            // 이동 중일 때는 차지 공격을 시작하지 않음
            if (moveDirection != Vector3.zero)
            {
                return;
            }
            // ▲▲▲ 여기까지 추가되었습니다! ▲▲▲

            isCharging = true;
            chargeTimer = 0f;
            anim.SetBool("isCharging", true);
        }
    }

    void HandleCharging()
    {
        chargeTimer += Time.deltaTime;

        if (Input.GetMouseButtonUp(1))
        {
            if (chargeTimer >= requiredChargeDuration)
            {
                isAttacking = true;
                anim.SetTrigger("ChargeAttack");
            }
            isCharging = false;
            anim.SetBool("isCharging", false);
        }
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    public void OnDodgeEnd()
    {
        isDodging = false;
    }
}