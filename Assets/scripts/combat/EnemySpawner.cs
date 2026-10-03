using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyPrefab;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minimumSpawnDistance = 8f;
    [SerializeField] private float maximumSpawnDistance = 12f;

    private Transform player;
    private float nextSpawnTime;

    private void Awake()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();

            nextSpawnTime =
                Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;

        float randomDistance =
            Random.Range(
                minimumSpawnDistance,
                maximumSpawnDistance
            );

        Vector2 spawnPosition =
            (Vector2)player.position +
            randomDirection * randomDistance;

        Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}