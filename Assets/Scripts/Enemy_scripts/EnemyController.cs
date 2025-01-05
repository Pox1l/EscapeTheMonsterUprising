using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target; // Cíl (hráè)
    public float speed; // Rychlost nepøítele
    private Animator animator; // Animator pro pohyb animací

    void Start()
    {
        animator = GetComponent<Animator>();
        FindPlayer(); // Najde hráèe pøi spuštìní
    }

    void Update()
    {
        if (target == null)
        {
            FindPlayer(); // Pokud target (hráè) zmizí, zkusí ho znovu najít
            return; // Poèkej na další frame
        }

        // Pohyb nepøítele smìrem k hráèi
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(target.position, transform.position);

        // Pohyb
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Aktualizace animace
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", distance > 0.1f ? speed : 0f);
    }

    // Najde hráèe podle tagu "Player"
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Hráè nebyl nalezen ve scénì!");
        }
    }
}
