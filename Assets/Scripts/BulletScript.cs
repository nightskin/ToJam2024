using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject owner;
    public float speed = 200;
    public Vector3 direction;
    public Transform homingTarget;
    public int damage = 10;
    public TrailRenderer trailRenderer;
    
    Vector3 prevPosition;
    [SerializeField] GameObject explosionPrefab;
    
    void Update()
    {
        prevPosition = transform.position;
        if (homingTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, homingTarget.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        if (Physics.Linecast(prevPosition, transform.position, out RaycastHit hit))
        {
            if(hit.transform.gameObject != owner)
            {
                Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                if(hit.transform.tag == "Asteroid")
                {
                    AsteroidGenerator generator = hit.transform.GetComponent<AsteroidGenerator>();
                    if(generator)
                    {
                        generator.RemoveBlock(hit.point);
                    }
                    Destroy(gameObject);
                }
                else if(hit.transform.tag == "Player")
                {
                    HealthScript health = hit.transform.GetComponent<HealthScript>();
                    if(health)
                    {
                        health.TakeDamage(damage);
                    }
                    Destroy(gameObject);
                }
                else if(hit.transform.tag == "Enemy" && owner.tag != "Enemy")
                {
                    ScoreScript.AddScore(10);
                    HealthScript health = hit.transform.GetComponent<HealthScript>();
                    if (health) 
                    { 
                        health.TakeDamage(damage);
                        if(health.IsDead())
                        {
                            Destroy(hit.transform.gameObject);
                            ScoreScript.AddScore(30);
                            SolarSystem.enemies.Remove(hit.transform.gameObject);
                        }
                    }
                    Destroy(gameObject);
                }

            }
        }
    }
}
