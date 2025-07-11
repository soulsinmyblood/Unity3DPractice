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
        // �Ϲ� ���� (���콺 ��Ŭ��)
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
        }
        // ȸ�� (�����̽���)
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isDodging = true;
            anim.SetTrigger("Dodge");
        }
        // ���� ���� (���콺 ��Ŭ�� ������ ����)
        else if (Input.GetMouseButtonDown(1))
        {
            // ���� �� �κ��� �߰��Ǿ����ϴ�! ����
            // �̵� ���� ���� ���� ������ �������� ����
            if (moveDirection != Vector3.zero)
            {
                return;
            }
            // ���� ������� �߰��Ǿ����ϴ�! ����

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