using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    Rigidbody2D rb;
    Vector2 moveDir;
    [SerializeField] private Animator animator; // Animator Referansı
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
    }

    void FixedUpdate()
    {
        Move();
        UpdateAnimation(moveDir);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (direction.x > 0)
        {
            animator.SetFloat("MoveDir", 1); // RightWalk animation
        }
        else if (direction.x < 0)
        {
            animator.SetFloat("MoveDir", -1); // LeftWalk animation
        }
        else
        {
            animator.SetFloat("MoveDir", 0); // Idle or no movement
        }
    }

    void InputManager(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDir = new Vector2(moveX,moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed , moveDir.y *moveSpeed);
    }
}
