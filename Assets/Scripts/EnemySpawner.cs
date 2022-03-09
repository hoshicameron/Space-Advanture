using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    [SerializeField] private GameObject[] enemies=null;
    [SerializeField] private float waitTime=0;
    [SerializeField] private int minSpawnCount;
    [SerializeField] private int maxSpawnCount;
    [SerializeField] private float delayBetweenSpawns;

    private float screenWidth;
    private List<GameObject> spawnedEnemies=new List<GameObject>();

    private void Start()
    {
        screenWidth = HelperMethods.GetScreenBounds(Camera.main).x;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {

        yield return new WaitForSeconds(waitTime);
        yield return SpawnNewWaveOfEnemies();

    }

    private IEnumerator SpawnNewWaveOfEnemies()
    {
        if (spawnedEnemies.Count > 0) yield break;

        int randomSpawnNumber = Random.Range(minSpawnCount, maxSpawnCount);
        for (int i = 0; i < randomSpawnNumber; i++)
        {
            Vector3 randomPosition=new Vector3(Random.Range(-screenWidth,screenWidth),
                transform.position.y + Random.Range(-0.5f,0.5f),0);
            GameObject newEnemy =
                PoolManager.Instance.ReuseGameObject(enemies[Random.Range(0, enemies.Length)], randomPosition,
                    Quaternion.identity) as GameObject;
            newEnemy.SetActive(true);
            spawnedEnemies.Add(newEnemy);

            yield return new WaitForSeconds(delayBetweenSpawns);
        }


        GameManager.Instance.UpdateSurvivedWave(1);
        CollectableSpawner.Instance.SpawnCollectables(transform.position);

    }

    public void CheckForSpawnNewWave(GameObject shipToRemove)
    {
        spawnedEnemies.Remove(shipToRemove);

        if (spawnedEnemies.Count==0)    StartCoroutine(SpawnWave());
    }
}
