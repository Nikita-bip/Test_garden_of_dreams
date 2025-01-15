using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;

    public float detectionRadius = 5f; // Радиус обнаружения игрока
    public float attackRange = 1f; // Радиус атаки игрока
    public float moveSpeed = 2f; // Скорость движения врага
    public float attackCooldown = 2f; // Время на восстановление атаки
    public int attackDamage = 10; // Урон, наносимый игроку
    public GameObject[] items; // Массив возможных предметов для выпадения
    [SerializeField] private int _maxHealth = 100;

    private Transform player; // Ссылка на игрока
    private PlayerController playerController; // Ссылка на скрипт игрока
    private bool isPlayerDetected = false; // Флаг для определения обнаружен ли игрок
    private bool isPlayerInRange = false; // Флаг для определения нахождения игрока в радиусе атаки
    private float nextAttackTime; // Время для следующей атаки
    private bool facingRight = true; // Флаг для определения направления взгляда врага
    public int currentHealth = 100; // Здоровье врага

    private void Start()
    {
        // Находим игрока по тегу "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        playerController = playerObject.GetComponent<PlayerController>();

        currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isPlayerDetected = true; // Обнаружен игрок
        }
        else
        {
            isPlayerDetected = false; // Игрок вне радиуса обнаружения
        }

        // Проверяем нахождение игрока в радиусе атаки
        isPlayerInRange = distanceToPlayer <= attackRange;

        if (isPlayerDetected && !isPlayerInRange)
        {
            // Если игрок обнаружен, но не в радиусе атаки, преследуем его
            ChasePlayer();
        }

        if (isPlayerInRange && Time.time > nextAttackTime)
        {
            // Если игрок в радиусе атаки и время следующей атаки наступило, атакуем
            AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        // Вычисляем направление к игроку
        Vector2 direction = (player.position - transform.position).normalized;

        // Двигаемся в направлении игрока с заданной скоростью
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Поворачиваем в направлении игрока
        if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
        {
            Flip();
        }
    }

    private void AttackPlayer()
    {
        // Наносим урон игроку
        playerController.TakeDamage(attackDamage);
        Debug.Log("Атака игрока! Урон нанесён: " + attackDamage);

        // Устанавливаем время для следующей атаки
        nextAttackTime = Time.time + attackCooldown;
    }

    private void Flip()
    {
        // Меняем направление взгляда врага
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            DropItem(); // Вызываем метод выпадения предмета при уничтожении врага
            Destroy(gameObject); // Уничтожаем врага
        }
    }

    private void DropItem()
    {
        if (items.Length > 0)
        {
            // Выбираем случайный предмет из массива
            GameObject itemToDrop = items[Random.Range(0, items.Length)];
            Instantiate(itemToDrop, transform.position, Quaternion.identity); // Создаём предмет
        }
    }

    // Рисуем радиусы обнаружения и атаки в редакторе (только для визуализации)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}