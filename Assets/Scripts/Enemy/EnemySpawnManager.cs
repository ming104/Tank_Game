using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] SpawnPoints;

    void Start()
    {
        StartCoroutine(EnemySpawnCo());
    }

    public IEnumerator EnemySpawnCo()
    {
        while (true)
        {
            var Enemy = Enemy_Pool.GetObject();
            Enemy.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
            yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        }
    }
}
