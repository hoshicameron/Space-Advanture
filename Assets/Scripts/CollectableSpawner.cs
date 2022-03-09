using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : SingletonMonoBehaviour<CollectableSpawner>
{
    [SerializeField] private GameObject[] collectables;

    public void SpawnCollectables(Vector3 position)
    {
        GameObject collectable = Random.Range(1, 10) > 7 ? collectables[0] : collectables[1];

        GameObject spawnedObject=PoolManager.Instance.ReuseGameObject(collectable, position, quaternion.identity);
        spawnedObject.SetActive(true);
    }
}
