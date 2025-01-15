using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _offset = 90f;
    public float speed = 5f;
    public int damage = 20;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // Устанавливаем скорость пули
        _rigidbody.velocity = transform.right * speed;
    }

    private void Update()
    {
        // Поворачиваем пулю в сторону её полёта
        float angle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + _offset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // Уничтожаем пулю после столкновения
        }
    }
}
