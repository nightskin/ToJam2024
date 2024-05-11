using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;

    [SerializeField] float turnFrequency = 5;
    [SerializeField] float moveSpeed = 100;
    [SerializeField] float fireRate = 1;


    float shootTimer = 0;
    float turnTimer = 0;
    Vector3 direction;
    CharacterController controller;
    Transform target;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        Aiming();
        Movement();
    }

    void Movement()
    {
        if (turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
        }
        else
        {
            direction = (target.position - transform.position).normalized;
            turnTimer = Random.Range(0, turnFrequency);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 10 * Time.deltaTime);
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
    }

    void Aiming()
    {
        if(Physics.SphereCast(bulletSpawn.position, 5, bulletSpawn.forward, out RaycastHit hit))
        {
            if(hit.transform.tag == "Player")
            {
                Shoot();
            }
        }
    }


    void Shoot()
    {
        if(shootTimer <= 0)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().owner = this.gameObject;
            bullet.GetComponent<BulletScript>().direction = (target.position - bulletSpawn.position).normalized;
            shootTimer = fireRate;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }
}
