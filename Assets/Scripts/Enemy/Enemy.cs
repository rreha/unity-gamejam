using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BulletType enum’unun doðru tanýmlandýðýndan emin olun

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxHealth = 10f;
    private float currentHealth;

    // Coroutine referanslarý
    private Coroutine fireCoroutine;
    private Coroutine waterCoroutine;
    private Coroutine steamCoroutine;

    // Fire efekti parametreleri
    private float fireDuration = 5f;
    private float fireDamagePerSecond = 1f;

    // Water efekti parametreleri
    private float waterDuration = 5f;

    // Steam efekti parametreleri
    private float steamDuration = 5f;
    private float speedMultiplier = 0.5f; // %50 hýz

    // Referanslar
    private EnemyMovement enemyMovement;

    // Efekt durum bayraklarý
    private bool isFireActive = false;
    private bool isWaterActive = false;
    private bool isSteamActive = false;

    void Start()
    {
        currentHealth = maxHealth;

        // EnemyMovement bileþenine referans al
        enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement == null)
        {
            Debug.LogError("EnemyMovement component not found on Enemy.");
        }

        LogCurrentEffects(); // Baþlangýçta logla
    }

    // Düþman mermi tarafýndan vurulduðunda çaðrýlan metod
    public void OnHit(BulletType bulletType, float damageAmount)
    {
        Debug.Log($"{gameObject.name} was hit by {bulletType} bullet for {damageAmount} damage.");

        switch (bulletType)
        {
            case BulletType.Fire:
                ApplyFireEffect();
                break;
            case BulletType.Water:
                ApplyWaterEffect();
                break;
            // Diðer mermi türlerini gerektiðinde ekleyebilirsiniz
            default:
                break;
        }

        // Merminin verdiði hasarý uygula
        Damage(damageAmount);

        // Efektlerin uygulanmasýný hemen kontrol et
        CheckAndApplySteam();
    }

    private void ApplyFireEffect()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(FireDamageOverTime());
        LogCurrentEffects(); // Fire efekti uygulandýðýnda logla
    }

    private IEnumerator FireDamageOverTime()
    {
        isFireActive = true;
        LogCurrentEffects(); // Fire efekti aktif olduðunda logla
        float elapsed = 0f;
        while (elapsed < fireDuration && isFireActive)
        {
            Damage(fireDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        isFireActive = false;
        fireCoroutine = null;
        LogCurrentEffects(); // Fire efekti süresi dolduðunda logla
        CheckAndApplySteam();
    }

    private void ApplyWaterEffect()
    {
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
        }
        waterCoroutine = StartCoroutine(WaterEffectDuration());
        LogCurrentEffects(); // Water efekti uygulandýðýnda logla
    }

    private IEnumerator WaterEffectDuration()
    {
        isWaterActive = true;
        LogCurrentEffects(); // Water efekti aktif olduðunda logla
        yield return new WaitForSeconds(waterDuration);
        isWaterActive = false;
        waterCoroutine = null;
        LogCurrentEffects(); // Water efekti süresi dolduðunda logla
        CheckAndApplySteam();
    }

    private void CheckAndApplySteam()
    {
        // Hem Fire hem de Water efektleri aktifse Steam uygulamaya baþla
        if (isFireActive && isWaterActive)
        {
            if (!isSteamActive)
            {
                ApplySteamEffect();
            }
        }
        else
        {
            // Eðer Steam aktif ve Fire veya Water artýk aktif deðilse Steam'i kaldýr
            if (isSteamActive)
            {
                StopSteamEffect();
            }
        }
    }

    private void ApplySteamEffect()
    {
        steamCoroutine = StartCoroutine(SteamEffectDuration());
        LogCurrentEffects(); // Steam efekti uygulandýðýnda logla
    }

    private IEnumerator SteamEffectDuration()
    {
        isSteamActive = true;
        LogCurrentEffects(); // Steam efekti aktif olduðunda logla

        // Hareket hýzýný yavaþlat
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(speedMultiplier);
        }

        yield return new WaitForSeconds(steamDuration);

        // Hareket hýzýný normale döndür
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f);
        }

        isSteamActive = false;
        steamCoroutine = null;
        LogCurrentEffects(); // Steam efekti süresi dolduðunda logla
    }

    private void StopSteamEffect()
    {
        if (steamCoroutine != null)
        {
            StopCoroutine(steamCoroutine);
            steamCoroutine = null;
        }
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f);
        }
        isSteamActive = false;
        LogCurrentEffects(); // Steam efekti kaldýrýldýðýnda logla
    }

    // IDamagable arayüzünden gelen Damage metodu
    public void Damage(float damageAmount)
    {
        TakeDamage(damageAmount);
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage, remaining health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        // Düþmanýn yok edilmesi, skor arttýrma, animasyon oynatma gibi iþlemleri burada yapabilirsiniz
        Destroy(gameObject);
    }

    // Efektlerin durumlarýný loglayan metot
    private void LogCurrentEffects()
    {
        List<string> activeEffects = new List<string>();

        if (isFireActive)
            activeEffects.Add("Fire");
        if (isWaterActive)
            activeEffects.Add("Water");
        if (isSteamActive)
            activeEffects.Add("Steam");

        string effectString = activeEffects.Count > 0 ? string.Join(", ", activeEffects) : "None";

        Debug.Log($"{gameObject.name} current effects: {effectString}");
    }
}
