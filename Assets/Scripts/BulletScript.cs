using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject owner;
    public float speed = 200;
    public Vector3 direction;
    public int damage = 10;
    public TrailRenderer trailRenderer;
    
    Vector3 prevPosition;
    [SerializeField] GameObject explosionPrefab;
    


    void Update()
    {
        prevPosition = transform.position;
        transform.position += direction * speed * Time.deltaTime;
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
                }
                else if(hit.transform.tag == "Player")
                {

                }
                else if(hit.transform.tag == "Enemy")
                {

                }
                Destroy(gameObject);
            }
        }
    }
}
