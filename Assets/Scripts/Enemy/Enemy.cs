using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject[] _dropItems;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _detectionRadius = 1.1f;
    [SerializeField] private float _attackRange = 0.6f;
    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private int _attackDamage = 10;

    private Transform _player;
    private PlayerController _playerController;
    private bool _isPlayerDetected = false;
    private bool _isPlayerInRange = false;
    private float _nextAttackTime;
    private bool _facingRight = true;
    private int _currentHealth;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _player = playerObject.transform;
        _playerController = playerObject.GetComponent<PlayerController>();

        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _detectionRadius)
        {
            _isPlayerDetected = true;
        }
        else
        {
            _isPlayerDetected = false;
        }

        _isPlayerInRange = distanceToPlayer <= _attackRange;

        if (_isPlayerDetected && !_isPlayerInRange)
        {
            ChasePlayer();
        }

        if (_isPlayerInRange && Time.time > _nextAttackTime)
        {
            AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, _player.position, _moveSpeed * Time.deltaTime);

        if ((direction.x < 0 && _facingRight) || (direction.x > 0 && !_facingRight))
        {
            Flip();
        }
    }

    private void AttackPlayer()
    {
        _playerController.TakeDamage(_attackDamage);
        _nextAttackTime = Time.time + _attackCooldown;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void DropItem()
    {
        if (_dropItems.Length > 0)
        {
            GameObject itemToDrop = _dropItems[Random.Range(0, _dropItems.Length)];
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}