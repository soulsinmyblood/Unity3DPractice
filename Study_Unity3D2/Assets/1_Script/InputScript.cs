using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Animator anim_Santa;

    void Update()
    {
        // �Է�
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // �̵�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ȸ��
        // �Է°�(moveDirection)�� 0�� �ƴѰ�쿡�� �츮�� �����̴°����� �Ǵ��Ѵ�.
        // moveDirection�� 0�϶��� - Idle �ִϸ��̼�
        // moveDirection�� 0�� �ƴѰ�� - Move �ִϸ��̼�

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim_Santa.SetTrigger("ATTACK");
        }

        // �ִϸ�����
        bool isWalk = 0 < moveDirection.magnitude;
        // moveDirection.magnitude : ������ ���̸� ��ȯ�մϴ�.
        // �Է� ���� ������ ������ ���̰� 0���� Ŀ���鼭 True�� ��ȯ�մϴ�.
        anim_Santa.SetBool("ISWALK", isWalk); // SetBool(�Ķ������ �̸�, ���� ��)

        // anicon_Santa.SetInteger() : Int
        // anicon_Santa.SetFloat() : Float
        // anicon_Santa.SetBool() : Bool
        // anicon_Santa.SetTrigger() : Trigger

        // anicon_PicoChan�̶�� �ִϸ����͸� ���� ������ �����մϴ�.
        // Bool Ÿ���� Parameter�� �����Ͽ��⿡ SetBool�Լ��� ����մϴ�.
    }
}
