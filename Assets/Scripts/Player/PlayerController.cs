using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint; 
    [SerializeField] private float _detectionRadius = 1f;

    private int _currentHealth;
    private Rigidbody2D _rigidbody;
    private bool _facingRight = true;
    private float _dirX, _dirY;
    private Transform _nearestEnemy;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        _dirX = _joystick.Horizontal * _speed;
        _dirY = _joystick.Vertical * _speed;

        AimAndShoot();

        if (_dirX > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_dirX < 0 && _facingRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_dirX, _dirY);
    }

    public void Shoot()
    {
        Item bulletItem = InventoryManager.Instance.Items.Find(item => item.id == 1);

        if (bulletItem != null && bulletItem.count > 0)
        {
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            InventoryManager.Instance.RemoveItem(bulletItem, 1);
        }
        else
        {
            Debug.Log("No bullets left!");
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        _healthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Destroy(gameObject);
        }
    }

    private void AimAndShoot()
    {
        _nearestEnemy = FindNearestEnemy();

        if (_nearestEnemy != null)
        {
            Vector2 direction = (_nearestEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (_dirX != 0 || _dirY != 0)
        {
            float angle = Mathf.Atan2(_dirY, _dirX) * Mathf.Rad2Deg;
            _firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius);

        Transform nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = collider.transform;
                }
            }
        }

        return nearestEnemy;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}