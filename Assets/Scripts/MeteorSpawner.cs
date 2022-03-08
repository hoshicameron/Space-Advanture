using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private MeteorController[] meteors=null;
    [SerializeField] private float minInterval=0;
    [SerializeField] private float maxInterval=0;
    [SerializeField] private int minSpawnCount=0;
    [SerializeField] private int maxSpawnCount=0;


    private float screenWidth;

    private void Start()
    {
        screenWidth = HelperMethods.GetScreenBounds(Camera.main).x;

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            int randomSpawnNumber = Random.Range(minSpawnCount, maxSpawnCount);
            for (int i = 0; i < randomSpawnNumber; i++)
            {
                Vector3 randomPosition=new Vector3(Random.Range(-screenWidth,screenWidth),transform.position.y,0);
                Instantiate(meteors[Random.Range(0, meteors.Length)],randomPosition,Quaternion.identity);
            }

            float randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);
        }

    }
}
