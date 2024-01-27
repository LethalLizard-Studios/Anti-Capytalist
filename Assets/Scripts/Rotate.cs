using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.up;
    [SerializeField] private float multiplier = 1.0f;

    private void Update()
    {
        transform.Rotate(direction * multiplier * Time.deltaTime, Space.Self);
    }
}
