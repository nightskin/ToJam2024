using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] Vector3 axis;
    void Start()
    {
        axis.x = Random.Range(0, 30);
        axis.y = Random.Range(0, 30);
        axis.z = Random.Range(0, 30);
    }

    void Update()
    {
        transform.Rotate(axis * Time.deltaTime);
    }
}
