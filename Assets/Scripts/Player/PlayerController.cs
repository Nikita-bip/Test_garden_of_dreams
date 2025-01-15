//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//public class PlayerController : MonoBehaviour
//{
//    [SerializeField] private float _speed = 1f;
//    [SerializeField] private int _maxHealth = 100;
//    [SerializeField] private Joystick _joystick;
//    [SerializeField] private HealthBar _healthBar;
//    [SerializeField] private GameObject bulletPrefab; // Префаб пули
//    [SerializeField] private Transform firePoint; // Точка выстрела
//    [SerializeField] private float detectionRadius = 5f; // Радиус обнаружения врагов

//    public int currentHealth;

//    private Rigidbody2D _rigidbody;
//    private bool facingRight = true; // Флаг для проверки направления

//    private float _dirX, _dirY;

//    private void Start()
//    {
//        _rigidbody = GetComponent<Rigidbody2D>();

//        currentHealth = _maxHealth;
//        _healthBar.SetMaxHealth(_maxHealth);
//    }

//    private void Update()
//    {
//        _dirX = _joystick.Horizontal * _speed;
//        _dirY = _joystick.Vertical * _speed;

//        AimAtNearestEnemy(); // Целимся в ближайшего врага

//        // Поворачиваем игрока только влево или вправо
//        if (_dirX > 0 && !facingRight)
//        {
//            Flip();
//        }
//        else if (_dirX < 0 && facingRight)
//        {
//            Flip();
//        }
//    }

//    private void FixedUpdate()
//    {
//        _rigidbody.velocity = new Vector2(_dirX, _dirY);
//    }

//    public void Shoot()
//    {
//        // Считаем патроны в инвентаре перед выстрелом
//        Item bulletItem = InventoryManager.Instance.Items.Find(item => item.id == 1); // Скорректируйте id

//        if (bulletItem != null && bulletItem.count > 0)
//        {
//            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//            InventoryManager.Instance.RemoveItem(bulletItem, 1);
//            Debug.Log("Player shot a bullet!");
//        }
//        else
//        {
//            Debug.Log("No bullets left!");
//        }
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

//    private void AimAtNearestEnemy()
//    {
//        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

//        Transform nearestEnemy = null;
//        float nearestDistance = Mathf.Infinity;

//        foreach (Collider2D collider in hitColliders)
//        {
//            if (collider.CompareTag("Enemy"))
//            {
//                float distance = Vector2.Distance(transform.position, collider.transform.position);
//                if (distance < nearestDistance)
//                {
//                    nearestDistance = distance;
//                    nearestEnemy = collider.transform;
//                }
//            }
//        }

//        if (nearestEnemy != null)
//        {
//            // Целимся в ближайшего врага и поворачиваем игрока в его направлении
//            Vector2 direction = (nearestEnemy.position - transform.position).normalized;
//            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

//            // Поворачиваем игрока только влево или вправо
//            if (direction.x > 0 && !facingRight)
//            {
//                Flip();
//            }
//            else if (direction.x < 0 && facingRight)
//            {
//                Flip();
//            }
//        }
//    }

//    private void Flip()
//    {
//        facingRight = !facingRight;
//        Vector3 scaler = transform.localScale;
//        scaler.x *= -1;
//        transform.localScale = scaler;
//    }

//    // Визуализируем радиус обнаружения в редакторе
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.green;
//        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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
    private bool facingRight = true; // Флаг для проверки направления

    private float _dirX, _dirY;
    private Transform nearestEnemy;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        _dirX = _joystick.Horizontal * _speed;
        _dirY = _joystick.Vertical * _speed;

        AimAndShoot(); // Целимся и стреляем

        // Поворачиваем игрока только влево или вправо
        if (_dirX > 0 && !facingRight)
        {
            Flip();
        }
        else if (_dirX < 0 && facingRight)
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
        // Считаем патроны в инвентаре перед выстрелом
        Item bulletItem = InventoryManager.Instance.Items.Find(item => item.id == 1); // Скорректируйте id

        if (bulletItem != null && bulletItem.count > 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            InventoryManager.Instance.RemoveItem(bulletItem, 1); // Уменьшите количество патронов на 1
            Debug.Log("Player shot a bullet!");
        }
        else
        {
            Debug.Log("No bullets left!");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        _healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }

    private void AimAndShoot()
    {
        nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            // Целимся в ближайшего врага и стреляем в его направлении
            Vector2 direction = (nearestEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (_dirX != 0 || _dirY != 0)
        {
            // Стреляем в направлении движения
            float angle = Mathf.Atan2(_dirY, _dirX) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private Transform FindNearestEnemy()
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

        return nearestEnemy;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Визуализируем радиус обнаружения в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}