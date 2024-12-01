using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    private Transform target;
    public float speed;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float speed = direction.magnitude;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed* Time.deltaTime);

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", speed);
    }


}
