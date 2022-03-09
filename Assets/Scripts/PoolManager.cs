using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    [System.Serializable]
    public class PoolObject
    {
        public int poolSize;
        public GameObject prefab;
    }

    [SerializeField] private List<PoolObject> Pools = new List<PoolObject>();
    [SerializeField] private Transform poolTransform;

    private Dictionary<int,Queue<GameObject>> poolDictionary=new Dictionary<int, Queue<GameObject>>();

    private void Start()
    {
        for (int i = 0; i <Pools.Count; i++)
        {
            CreatePool(Pools[i].prefab, Pools[i].poolSize);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize)
    {
        print("Create Pool");
        int poolKey=prefab.GetInstanceID();
        string prefabName = prefab.name;

        GameObject parentGameObject= new GameObject(prefabName+"Anchor");

        parentGameObject.transform.parent = poolTransform;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey,new Queue<GameObject>());
            for (int i = 0; i < poolSize; i++)
            {
                GameObject newGameObject=Instantiate(prefab,parentGameObject.transform) as GameObject;
                newGameObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newGameObject);
            }
        }
    }

    public GameObject ReuseGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            GameObject objectToReuse = GetObjectFronPool(poolKey);
            ResetObject(position, rotation, objectToReuse, prefab);
            return objectToReuse;
        }else
        {
            Debug.Log("No Object pool for"+ prefab);
            return null;
        }
    }

    private GameObject GetObjectFronPool(int poolKey)
    {
        GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(objectToReuse);

        if(objectToReuse.activeSelf)
            objectToReuse.SetActive(false);

        return objectToReuse;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, GameObject objectToReuse, GameObject prefab)
    {
        objectToReuse.transform.position = position;
        objectToReuse.transform.rotation = rotation;

        objectToReuse.transform.localScale = prefab.transform.localScale;
    }
}
