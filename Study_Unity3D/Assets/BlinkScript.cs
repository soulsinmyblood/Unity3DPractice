using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        // ������ ������ ������Ʈ�� ��ġ�Ǿ�� �Ѵ�.

        transform.position = new Vector3(Random.Range(0f, 10f), 0f, Random.Range(0f, 10f)); // ������Ʈ ��ġ
    }
}
