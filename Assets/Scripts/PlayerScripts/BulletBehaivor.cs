using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaivor : MonoBehaviour
{
    [SerializeField] public float bullet_Speed = 10f;
    [SerializeField] public float bullet_Range = 10f; // Merminin menzili

    public LayerMask whatDestroysBullet;
    

    private Rigidbody2D rb;
    private Vector3 startPosition; // Merminin başlangıç pozisyonu
    public float bulletdamage = 5f;
    SpriteRenderer rbSprite;
    public enum BulletType
    {
        fire,
        electric,
        water
    }
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
        if((whatDestroysBullet.value & (1<< collision.gameObject.layer))> 0)
        {
            //spawn particles

            //play sound FX

            //ScreenShake gíbi seyler eklenebilir

            //damage enemy
            IDamagable iDamageable = collision.gameObject.GetComponent<IDamagable>();
           if(iDamageable!= null)
            {
               iDamageable.Damage(bulletdamage);
            }


            //destroy the bullet
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
