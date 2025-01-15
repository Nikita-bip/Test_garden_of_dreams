using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab; 
    [SerializeField] private Vector2 _spawnAreaMin;
    [SerializeField] private Vector2 _spawnAreaMax;

    private int numberOfEnemies = 3;

    private void Start()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float spawnX = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
        float spawnY = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
    }
}