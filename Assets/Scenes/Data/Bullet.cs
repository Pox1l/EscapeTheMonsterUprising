using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private int damage;

    void Update()
    {
        // Pohyb støely vpøed
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public void SetDamage(int damageValue)
    {
        damage = damageValue;
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
            Destroy(gameObject); // Zniè støelu po zásahu
        }
    }
}
