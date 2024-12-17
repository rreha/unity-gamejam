using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum BulletType
{
    fire,
    electric,
    water
}

public class BulletBehaivor : MonoBehaviour
{
    [SerializeField] public float bullet_Speed = 10f;
    [SerializeField] public float bullet_Range = 10f; // Merminin menzili

    public LayerMask whatDestroysBullet;
    

    private Rigidbody2D rb;
    private Vector3 startPosition; // Merminin başlangıç pozisyonu
    public float bulletdamage = 10f;
    SpriteRenderer rbSprite;

    public BulletType bulletType;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
        //set velocicty based on bullet type
        InitializeBulletStats();
        startPosition = transform.position; // Merminin başlangıç pozisyonunu kaydet
        
    }

    private void InitializeBulletStats()
    {
        
            SetStraightVelocity();
    }
    private void Update()
    {
        CheckRange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnHit(bulletType, bulletdamage);
        }

        // Mermi çarptıktan sonra yok oluyor
        Destroy(gameObject);
    }

    private IEnumerator ApplyFireDamage(IDamagable target)
    {
        float duration = 5f; // Duration of the fire effect in seconds
        float interval = 1f; // Damage interval in seconds
        int ticks = Mathf.FloorToInt(duration / interval); // Number of times to apply damage

        for (int i = 0; i < ticks; i++)
        {
            target.Damage(bulletdamage / ticks); // Apply damage per tick
            yield return new WaitForSeconds(interval); // Wait before applying next damage
        }
    }

    private void CheckRange()
    {
        // Başlangıç pozisyonundan ne kadar uzaklaştığını kontrol et
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        // Eğer menzili aşarsa, mermiyi yok et
        if (distanceTraveled >= bullet_Range)
        {
            Destroy(gameObject);
        }
    }
    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * bullet_Speed;
    }
}
