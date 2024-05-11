using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Min(0)] public float camDistance = 10;
    public Vector3 offset;
    [SerializeField] Transform target;


    void Start()
    {
        if (!target) target = GameObject.Find("Player").transform;
    }
    
    void Update()
    {
        Vector3 pos = target.position - transform.forward * camDistance;

        if(camDistance == 0)
        {
            transform.position = pos + offset;
        }
        else 
        {
            transform.position = Vector3.Lerp(transform.position, pos + offset, 10 * Time.deltaTime);
        }


    }
}
