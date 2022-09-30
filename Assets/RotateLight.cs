using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    [Range(0.0F, 1.0F)]
    public float Speed = 1.0f;
    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0, Speed, 0);
    }
}
