using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    public float baseMoveSpeed = 5f; // Temel hareket hızı
    private float currentMoveSpeed; // Geçerli hareket hızı
    private Rigidbody2D rb; // Rigidbody2D referansı
    private Vector2 movement; // Hareket yönü

    void Start()
    {
        // Oyuncuyu bul
        player = FindObjectOfType<PlayerMovement>().transform;

        // Rigidbody2D bileşenini al
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on Enemy.");
        }

        // Geçerli hareket hızını başlat
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        // Oyuncuya doğru yönü hesapla
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;
    }

    void FixedUpdate()
    {
        // Hareketi uygula
        rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.fixedDeltaTime);
    }

    // Hareket hızını değiştiren metod
    public void SetSpeedMultiplier(float multiplier)
    {
        currentMoveSpeed = baseMoveSpeed * multiplier;
        Debug.Log($"{gameObject.name} speed multiplier set to {multiplier}. Current speed: {currentMoveSpeed}");
    }
}