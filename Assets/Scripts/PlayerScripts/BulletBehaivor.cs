using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    none,
    fire,
    water,
    electric
}
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] public float bullet_Speed = 30f;
    [SerializeField] public float bullet_Range = 10f; // Merminin menzili

    public LayerMask whatDestroysBullet;

    private Rigidbody2D rb;
    private Vector3 startPosition; // Merminin başlangıç pozisyonu
    public float bulletDamage = 10f;
    SpriteRenderer rbSprite;
    public BulletType bulletType;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
        // Merminin yönünü ve hızını ayarla
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
            enemy.OnHit(bulletType, bulletDamage);
            // Mermi çarptıktan sonra yok oluyor
            Destroy(gameObject);
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
