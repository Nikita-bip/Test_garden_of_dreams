using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int numberOfEnemies = 3; 
    public Vector2 spawnAreaMin; 
    public Vector2 spawnAreaMax; 

    private void Start()
    {
        // ������� �������� ���������� ������ ��� ������ ����
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // ���������� ��������� ���������� � �������� ��������� ���������
        float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // ������ ����� � ��������� �������
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
