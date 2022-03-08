using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : SingletonMonoBehaviour<CollectableSpawner>
{
    [SerializeField] private Collectable[] collectables;

    public void SpawnCollectables(Vector3 position)
    {
        Collectable collectable = Random.Range(1, 10) > 7 ? collectables[0] : collectables[1];
        Instantiate(collectable, position, quaternion.identity);
    }
}
