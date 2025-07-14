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
        // Update������ Ű���� �Է°� ���� �� ������ Ȯ���ؾ� �ϴ� �͵��� ó���մϴ�.
        // �̵� ������ ����ϰ�, �ִϸ��̼� �Ķ���͸� �����ϴ� �ͱ����� ���⼭ �մϴ�.
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        anim.SetBool("isWalking", moveDirection != Vector3.zero);

        // ���� Ű �Է� Ȯ��
        HandleGuardInput();

        // ���¿� ���� �ൿ ���� (���� ȿ���� ���� ���� �κ�)
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
        // FixedUpdate������ ���� ȿ���� ���õ� ���� ó���մϴ�.
        // ���� ĳ���͸� �����̴� ���� ���⼭ ó���ؾ� �մϴ�.
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
            rb.MovePosition(rb.position + moveDirection * currentMoveSpeed * Time.fixedDeltaTime); // Time.deltaTime ��� Time.fixedDeltaTime ���
            rb.MoveRotation(Quaternion.LookRotation(moveDirection)); // transform.rotation ��� rb.MoveRotation ���
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

    // �� �Լ��� isCharging�� true�� ���� ȣ��Ǿ�� �ϹǷ�, Update���� ���� ȣ���ϴ� ���
    // HandleActionInitiation �Լ����� isCharging = true;�� �ٲ� �ĺ��� �ڿ�������
    // isCharging�� üũ�ϴ� ������ �ʿ�. ���� �������� ȸ��

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    public void OnDodgeEnd()
    {
        isDodging = false;
    }
}