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
                enemy.TakeDamage(damage); 
            }
            Destroy(gameObject); 
        }
    }
}
