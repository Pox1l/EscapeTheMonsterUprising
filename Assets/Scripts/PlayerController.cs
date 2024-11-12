using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;


    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;


    public int maxHealth = 100;
    public int currentHealth;

    public int expPoints = 0;
    public int expToLevelUp = 100;

    public Slider healthBar;
    public Slider expBar;

    public int damageToEnemy = 10;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        expBar.maxValue = expToLevelUp;
        expBar.value = expPoints;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        movement.y = Input.GetAxisRaw("Vertical") * moveSpeed;

        animator.SetFloat("Horizontal",movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);



        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Simulate attacking the enemy
            EnemyController enemy = FindObjectOfType<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageToEnemy);
                GainExp(20); // Gain EXP for attacking
            }
        }

        // Check if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
    }

    private void GainExp(int exp)
    {
        expPoints += exp;
        expBar.value = expPoints;

        if (expPoints >= expToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        expPoints = 0;
        expBar.value = expPoints;
        expToLevelUp += 50;
        maxHealth += 20;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        Debug.Log("Player Leveled Up!");
    }

    private void Die()
    {
        Debug.Log("Player is dead.");
        gameObject.SetActive(false);
    }
}