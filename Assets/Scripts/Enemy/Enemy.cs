using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;

    public float detectionRadius = 5f; // ������ ����������� ������
    public float attackRange = 1f; // ������ ����� ������
    public float moveSpeed = 2f; // �������� �������� �����
    public float attackCooldown = 2f; // ����� �� �������������� �����
    public int attackDamage = 10; // ����, ��������� ������
    public GameObject[] items; // ������ ��������� ��������� ��� ���������
    [SerializeField] private int _maxHealth = 100;

    private Transform player; // ������ �� ������
    private PlayerController playerController; // ������ �� ������ ������
    private bool isPlayerDetected = false; // ���� ��� ����������� ��������� �� �����
    private bool isPlayerInRange = false; // ���� ��� ����������� ���������� ������ � ������� �����
    private float nextAttackTime; // ����� ��� ��������� �����
    private bool facingRight = true; // ���� ��� ����������� ����������� ������� �����
    public int currentHealth = 100; // �������� �����

    private void Start()
    {
        // ������� ������ �� ���� "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        playerController = playerObject.GetComponent<PlayerController>();

        currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        // ��������� ���������� �� ������
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isPlayerDetected = true; // ��������� �����
        }
        else
        {
            isPlayerDetected = false; // ����� ��� ������� �����������
        }

        // ��������� ���������� ������ � ������� �����
        isPlayerInRange = distanceToPlayer <= attackRange;

        if (isPlayerDetected && !isPlayerInRange)
        {
            // ���� ����� ���������, �� �� � ������� �����, ���������� ���
            ChasePlayer();
        }

        if (isPlayerInRange && Time.time > nextAttackTime)
        {
            // ���� ����� � ������� ����� � ����� ��������� ����� ���������, �������
            AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        // ��������� ����������� � ������
        Vector2 direction = (player.position - transform.position).normalized;

        // ��������� � ����������� ������ � �������� ���������
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // ������������ � ����������� ������
        if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
        {
            Flip();
        }
    }

    private void AttackPlayer()
    {
        // ������� ���� ������
        playerController.TakeDamage(attackDamage);
        Debug.Log("����� ������! ���� ������: " + attackDamage);

        // ������������� ����� ��� ��������� �����
        nextAttackTime = Time.time + attackCooldown;
    }

    private void Flip()
    {
        // ������ ����������� ������� �����
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
            DropItem(); // �������� ����� ��������� �������� ��� ����������� �����
            Destroy(gameObject); // ���������� �����
        }
    }

    private void DropItem()
    {
        if (items.Length > 0)
        {
            // �������� ��������� ������� �� �������
            GameObject itemToDrop = items[Random.Range(0, items.Length)];
            Instantiate(itemToDrop, transform.position, Quaternion.identity); // ������ �������
        }
    }

    // ������ ������� ����������� � ����� � ��������� (������ ��� ������������)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}