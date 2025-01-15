using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarPoolManager : MonoBehaviour
{
    public static HealthbarPoolManager Instance;
    public GameObject healthbarPrefab;
    public Transform healthbarCanvas;
    public int poolSize = 20;

    private Queue<GameObject> healthbarPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject healthbar = Instantiate(healthbarPrefab, healthbarCanvas);
            healthbar.SetActive(false);
            healthbarPool.Enqueue(healthbar);
        }
    }

    public GameObject GetHealthbar()
    {
        if (healthbarPool.Count > 0)
        {
            GameObject healthbar = healthbarPool.Dequeue();
            healthbar.SetActive(true);
            return healthbar;
        }

        // Если пул пуст, создаем новый объект
        GameObject newHealthbar = Instantiate(healthbarPrefab, healthbarCanvas);
        return newHealthbar;
    }

    public void ReturnHealthbar(GameObject healthbar)
    {
        healthbar.SetActive(false);
        healthbarPool.Enqueue(healthbar);
    }
}