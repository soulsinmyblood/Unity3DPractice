using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        // 랜덤한 구간에 오브젝트가 배치되어야 한다.

        transform.position = new Vector3(Random.Range(0f, 10f), 0f, Random.Range(0f, 10f)); // 오브젝트 위치
    }
}
