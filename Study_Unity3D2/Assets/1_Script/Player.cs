using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Transform character;
    [SerializeField] Animator anicon;
    [SerializeField] float moveSpeed; // �̵� �ӵ�

    Vector2 moveInput; // �Է¹��� �̵� ������ ����� ����

    public float jumpPower; // ������
    public int MaxJumpCount; // �ִ� ���� Ƚ��
    [SerializeField] int nowJumpCount; // ���� ���� Ƚ��

    void Awake()
    {
        nowJumpCount = MaxJumpCount;
    }

    void Jump()
    {
        // Space Ű�� ������ + jumpCount�� 0���� ũ�� => �����Ѵ�.
        if (Input.GetKeyDown(KeyCode.Space) && 0 < nowJumpCount)
        {
            rigid.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
        }

        // [ rigid.velocity.y <= 0 ]
        // �����Ͽ� ��ü�� �ö󰥶��� ������Ʈ�� �������� �ӵ�(velocity)�� 0���� Ŭ ���Դϴ�.
        // �ݴ�� �������� �ӵ��� 0���� �۴ٸ�, �����ϰų� �������� �ִ� ��Ȳ�� ���Դϴ�.

        // [ Physics.Raycast(character.position, Vector3.down, 0.1f, LayerNumber.Ground) ]
        // Raycast�� ������ �ʴ� ��(Ray)�� ������ ���� ��� �κ��� �ľ��մϴ�.

        // Physics.Raycast��
        // - character.position + (Vector3.up * 0.1f) : �������� Collider�� ��ġ�� �ʵ��� 0.1f ��ŭ ���� �����մϴ�.
        // - Vector3.down : �Ʒ� ��������
        // - 0.2f : 0.2f ��ŭ�� ũ�⸸ŭ Ž���Ͽ��� ��
        // - LayerNumber.Ground : ���� ��´ٸ�
        // True�� ��ȯ�մϴ�.

        // �ᱹ �������ų� ������ ���� + ���� ĳ���� �Ÿ��� 0.1f ���ϸ� => ���� Ƚ���� �ʱ�ȭ�մϴ�.
        if (rigid.velocity.y <= 0 && Physics.Raycast(character.position + (Vector3.up * 0.1f), Vector3.down, 0.2f, LayerMask.GetMask("Ground")))
        {
            nowJumpCount = MaxJumpCount;
        }
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        // �Է�
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.x = Mathf.MoveTowards(moveInput.x, rawInput.x, Time.deltaTime * 10);
        moveInput.y = Mathf.MoveTowards(moveInput.y, rawInput.y, Time.deltaTime * 10);
        float moveValue = moveInput.magnitude;

        // �̵�
        if (moveValue != 0)
        {
            Vector3 inputForward = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            rigid.MovePosition(transform.position + (inputForward * Time.deltaTime * moveSpeed));

            if (moveInput != Vector2.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputForward);
                character.rotation = Quaternion.Slerp(character.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // �ִϸ��̼�
        anicon.SetBool("ISWALK", moveValue != 0);
    }

    
}