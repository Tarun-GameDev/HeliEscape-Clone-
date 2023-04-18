using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int noOfEnemies = 5;
    [SerializeField] Collider col;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnRandomOffset = 2f;
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform AllEnemiesHolder;

    int i;

    private void Start()
    {
        if (col == null)
            col = GetComponent<Collider>();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < noOfEnemies; i++)
        {
            Instantiate(enemyPrefab, new Vector3(spawnPos.position.x + Random.Range(-spawnRandomOffset, spawnRandomOffset), spawnPos.position.y + 1.5f, spawnPos.position.z + Random.Range(-spawnRandomOffset, spawnRandomOffset)),Quaternion.identity);
        }
    }

    IEnumerator SpawnEnemy()
    {
        var obj = Instantiate(enemyPrefab, new Vector3(spawnPos.position.x + Random.Range(-spawnRandomOffset, spawnRandomOffset), spawnPos.position.y + 1.5f, spawnPos.position.z + Random.Range(-spawnRandomOffset, spawnRandomOffset)), Quaternion.identity);
        obj.transform.SetParent(AllEnemiesHolder.transform);
        i++;
        yield return new WaitForSeconds(.1f);
        if (i <= noOfEnemies)
            if(!LadyController.ladyDead)
                StartCoroutine(SpawnEnemy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Lady"))
        {
            StartCoroutine(SpawnEnemy());
        }
    }
}
