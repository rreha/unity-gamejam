using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnAreaWidth = 10f; // Spawn alanı genişliği
    [SerializeField] private float spawnAreaHeight = 10f; // Spawn alanı yüksekliği
    [SerializeField] private float spawnInterval = 5f; // Tekrar spawnlanasıya geçen süre

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
            0f,
            Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2)
        );

        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}
