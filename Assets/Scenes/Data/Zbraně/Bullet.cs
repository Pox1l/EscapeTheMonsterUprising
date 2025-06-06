using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Smìr od nepøítele ke kulce (opaèný smìr zásahu)
                Vector2 hitDirection = (transform.position - collision.transform.position).normalized;

                enemy.TakeDamage(damage, hitDirection);
            }
            Destroy(gameObject);
        }
    }
}
