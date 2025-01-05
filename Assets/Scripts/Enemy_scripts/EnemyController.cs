using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target; // C�l (hr��)
    public float speed; // Rychlost nep��tele
    private Animator animator; // Animator pro pohyb animac�

    void Start()
    {
        animator = GetComponent<Animator>();
        FindPlayer(); // Najde hr��e p�i spu�t�n�
    }

    void Update()
    {
        if (target == null)
        {
            FindPlayer(); // Pokud target (hr��) zmiz�, zkus� ho znovu naj�t
            return; // Po�kej na dal�� frame
        }

        // Pohyb nep��tele sm�rem k hr��i
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(target.position, transform.position);

        // Pohyb
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Aktualizace animace
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", distance > 0.1f ? speed : 0f);
    }

    // Najde hr��e podle tagu "Player"
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Hr�� nebyl nalezen ve sc�n�!");
        }
    }
}
