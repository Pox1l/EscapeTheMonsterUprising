using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Pole, které chceme upravovat přes upgrade manager
    [SerializeField]
    private float _moveSpeed = 5f;

    // Veřejná vlastnost pro bezpečný přístup k rychlosti pohybu
    public float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * _moveSpeed * Time.fixedDeltaTime);
    }
}
