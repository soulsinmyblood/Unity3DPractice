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

        // 1. �̵��� �׻� ó�� (���� �߿��� �ȱ� �Ķ���Ͱ� ���ŵǵ���)
        HandleMovement();

        // 2. ���� Ű �Է��� Ȯ��
        HandleGuardInput();

        // 3. ���� ���¿� ���� �ൿ�� ����
        // ȸ�� ���� ���� �������� ĵ�� ����
        if (isDodging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDodging = false;
                isAttacking = true;
                anim.SetTrigger("Attack");
            }
            return; // ȸ�� �߿��� �ٸ� �ൿ �Ұ�
        }

        // ����, ����, ���� ���� ���� ���ο� �ൿ�� ������ �� ����
        if (isAttacking || isCharging || isGuarding)
        {
            return;
        }

        // ��� ������ �ƴ� ���� ���ο� �ൿ(����, ȸ��, ����)�� ����
        HandleActionInitiation();
    }

    void HandleGuardInput()
    {
        isGuarding = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("isGuarding", isGuarding);
    }

    void HandleMovement()
    {
        // ���� ���� �ƴ� ���� Ű �Է��� �޾Ƽ� �̵� ������ ����
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

    // ���ο� �ൿ�� '����'�ϴ� �͸� ����ϴ� �Լ�
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