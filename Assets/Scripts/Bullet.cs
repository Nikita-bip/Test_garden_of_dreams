using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _offset = 90f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _damage = 20;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.right * _speed;
    }

    private void Update()
    {
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
                enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}