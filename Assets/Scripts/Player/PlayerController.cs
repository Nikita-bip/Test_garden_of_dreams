//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//public class PlayerController : MonoBehaviour
//{
//    [SerializeField] private float _speed = 1f;
//    [SerializeField] private int _maxHealth = 100;
//    [SerializeField] private Joystick _joystick;
//    [SerializeField] private HealthBar _healthBar;

//    public int currentHealth;

//    private Rigidbody2D _rigidbody;

//    private float _dirX, _dirY;

//    private void Start()
//    {
//        _rigidbody = GetComponent<Rigidbody2D>();

//        currentHealth = _maxHealth;
//        _healthBar.SetMaxHealth(_maxHealth);
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            TakeDamage(20);
//        }

//        _dirX = _joystick.Horizontal * _speed;
//        _dirY = _joystick.Vertical * _speed;
//    }

//    private void FixedUpdate()
//    {
//        _rigidbody.velocity = new Vector2(_dirX, _dirY);
//    }

//    public void TakeDamage(int damage)
//    {
//        currentHealth -= damage;

//        _healthBar.SetHealth(currentHealth);

//        if (currentHealth <= 0)
//        {
//            currentHealth = 0;
//            Debug.Log("Dead");
//        }
//    }
//}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject bulletPrefab; // Префаб пули
    [SerializeField] private Transform firePoint; // Точка выстрела
    [SerializeField] private float detectionRadius = 5f; // Радиус обнаружения врагов

    public int currentHealth;

    private Rigidbody2D _rigidbody;

    private float _dirX, _dirY;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        _dirX = _joystick.Horizontal * _speed;
        _dirY = _joystick.Vertical * _speed;

        AimAtNearestEnemy(); // Целимся в ближайшего врага
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_dirX, _dirY);
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("Player shot a bullet!");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        _healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead");
        }
    }

    private void AimAtNearestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

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

        if (nearestEnemy != null)
        {
            // Целимся в ближайшего врага
            Vector2 direction = (nearestEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // Визуализируем радиус обнаружения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

