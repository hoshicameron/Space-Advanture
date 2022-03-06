using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    [SerializeField] private GameObject[] enemies=null;
    [SerializeField] private float waitTime=0;
    [SerializeField] private int minSpawnCount;
    [SerializeField] private int maxSpawnCount;

    private float screenWidth;
    public List<GameObject> spawnedEnemies;

    private void Start()
    {
        screenWidth = HelperMethods.GetScreenBounds(Camera.main).x;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {

        yield return new WaitForSeconds(waitTime);
        SpawnNewWaveOfEnemies();

    }

    private void SpawnNewWaveOfEnemies()
    {
        if (spawnedEnemies.Count > 0) return;

        int randomSpawnNumber = Random.Range(minSpawnCount, maxSpawnCount);
        for (int i = 0; i < randomSpawnNumber; i++)
        {
            Vector3 randomPosition=new Vector3(Random.Range(-screenWidth,screenWidth),
                transform.position.y + Random.Range(-0.5f,0.5f),0);
            GameObject newEnemy =
                Instantiate(enemies[Random.Range(0, enemies.Length)], randomPosition,
                    Quaternion.identity) as GameObject;

            spawnedEnemies.Add(newEnemy);
        }


        // Todo Increase UI wave count

    }

    public void CheckForSpawnNewWave(GameObject shipToRemove)
    {
        spawnedEnemies.Remove(shipToRemove);

        if (spawnedEnemies.Count==0)    StartCoroutine(SpawnWave());
    }
}
