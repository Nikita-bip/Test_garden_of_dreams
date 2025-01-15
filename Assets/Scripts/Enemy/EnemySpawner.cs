using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int numberOfEnemies = 3; 
    public Vector2 spawnAreaMin; 
    public Vector2 spawnAreaMax; 

    private void Start()
    {
        // Спавним заданное количество врагов при старте игры
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Генерируем случайные координаты в пределах заданного диапазона
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Создаём врага в случайной позиции
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
