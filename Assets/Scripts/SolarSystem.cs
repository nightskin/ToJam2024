using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public static Noise noise;
    public static List<GameObject> enemies = new List<GameObject>();
    public string seed = "";

    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] float spawnRadius = 2500;
    [SerializeField] int numberOfAsteroids = 10;
    [SerializeField] int maxEnemies = 30;

    [SerializeField] float spawnFrequency = 5;
    float spawnTimer;

    void Awake()
    {
        spawnTimer = spawnFrequency;
        if(seed == string.Empty) seed = Random.Range(int.MinValue, int.MaxValue).ToString();
        noise = new Noise(seed.GetHashCode());
        Random.InitState(seed.GetHashCode());

        if(numberOfAsteroids <= 0)
        {
            numberOfAsteroids = 10;
        }
        for(int i = 0; i < numberOfAsteroids; i++) 
        {
            Vector3 pos = Random.insideUnitSphere * spawnRadius;
            Instantiate(asteroidPrefab, pos, Quaternion.identity, transform);
        }

        for (int i = 0; i < maxEnemies; i++) 
        {
            int enemyIndex = Random.Range(0, enemyTypes.Length);
            Vector3 pos = Random.insideUnitSphere * spawnRadius;
            var e = Instantiate(enemyTypes[enemyIndex], pos, Quaternion.identity, transform);
            enemies.Add(e);
        }

    }

    void Update()
    {
        if(enemies.Count < maxEnemies) 
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer < 0)
            {
                int enemyIndex = Random.Range(0, enemyTypes.Length);
                Vector3 pos = Random.insideUnitSphere * spawnRadius;
                var e = Instantiate(enemyTypes[enemyIndex], pos, Quaternion.identity, transform);
                enemies.Add(e);
            }
        }       
    }

}
