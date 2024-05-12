using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] int numberOfObjects = 50;
    [SerializeField] GameObject prefab = null;
    [SerializeField] GameObject[] prefabs = null;

    List<GameObject> pooledObjects = new List<GameObject>();


    void Awake()
    {
        if (prefab)
        {
            for(int i = 0; i < numberOfObjects; i++) 
            {
                GameObject obj = Instantiate(prefab, transform);
                pooledObjects.Add(obj);
                obj.SetActive(false);
            }
        }
        else if(prefabs != null)
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                int r = Random.Range(0, prefabs.Length);
                GameObject obj = Instantiate(prefabs[r], transform);
                pooledObjects.Add(obj);
                obj.SetActive(false);
            }
        }
    }

    public GameObject Spawn(Vector3 position)
    {
        GameObject obj = GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = position;
            obj.SetActive(true);
        }
        return obj;
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0;i < pooledObjects.Count;i++)
        {
            if (!pooledObjects[i].activeSelf)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
