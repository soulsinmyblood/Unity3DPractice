using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision Enter : {collision.gameObject.name}");
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"Collision Stay : {collision.gameObject.name}");
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Collision Exit : {collision.gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Enter : {other.gameObject.name}");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log($"Trigger Stay : {other.gameObject.name}");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger Exit : {other.gameObject.name}");
    }
}
