using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    public float moveSpeed; // Hareket hızı
    private Rigidbody2D rb; // RigidBody referansı
    private Vector2 movement; // Hareket yönü

    void Start()
    {
        // Player'ı bul
        player = FindObjectOfType<PlayerMovement>().transform;

        // RigidBody'yi al
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Oyuncuya doğru yönü hesapla
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;
    }

    void FixedUpdate()
    {
        // RigidBody kullanarak hareketi uygula
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
