using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BulletType enum�unun do�ru tan�mland���ndan emin olun

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxHealth = 10f;
    private float currentHealth;

    // Coroutine referanslar�
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
    private float speedMultiplier = 0.5f; // %50 h�z

    // Referanslar
    private EnemyMovement enemyMovement;

    // Efekt durum bayraklar�
    private bool isFireActive = false;
    private bool isWaterActive = false;
    private bool isSteamActive = false;

    void Start()
    {
        currentHealth = maxHealth;

        // EnemyMovement bile�enine referans al
        enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement == null)
        {
            Debug.LogError("EnemyMovement component not found on Enemy.");
        }

        LogCurrentEffects(); // Ba�lang��ta logla
    }

    // D��man mermi taraf�ndan vuruldu�unda �a�r�lan metod
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
            // Di�er mermi t�rlerini gerekti�inde ekleyebilirsiniz
            default:
                break;
        }

        // Merminin verdi�i hasar� uygula
        Damage(damageAmount);

        // Efektlerin uygulanmas�n� hemen kontrol et
        CheckAndApplySteam();
    }

    private void ApplyFireEffect()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(FireDamageOverTime());
        LogCurrentEffects(); // Fire efekti uyguland���nda logla
    }

    private IEnumerator FireDamageOverTime()
    {
        isFireActive = true;
        LogCurrentEffects(); // Fire efekti aktif oldu�unda logla
        float elapsed = 0f;
        while (elapsed < fireDuration && isFireActive)
        {
            Damage(fireDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        isFireActive = false;
        fireCoroutine = null;
        LogCurrentEffects(); // Fire efekti s�resi doldu�unda logla
        CheckAndApplySteam();
    }

    private void ApplyWaterEffect()
    {
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
        }
        waterCoroutine = StartCoroutine(WaterEffectDuration());
        LogCurrentEffects(); // Water efekti uyguland���nda logla
    }

    private IEnumerator WaterEffectDuration()
    {
        isWaterActive = true;
        LogCurrentEffects(); // Water efekti aktif oldu�unda logla
        yield return new WaitForSeconds(waterDuration);
        isWaterActive = false;
        waterCoroutine = null;
        LogCurrentEffects(); // Water efekti s�resi doldu�unda logla
        CheckAndApplySteam();
    }

    private void CheckAndApplySteam()
    {
        // Hem Fire hem de Water efektleri aktifse Steam uygulamaya ba�la
        if (isFireActive && isWaterActive)
        {
            if (!isSteamActive)
            {
                ApplySteamEffect();
            }
        }
        else
        {
            // E�er Steam aktif ve Fire veya Water art�k aktif de�ilse Steam'i kald�r
            if (isSteamActive)
            {
                StopSteamEffect();
            }
        }
    }

    private void ApplySteamEffect()
    {
        steamCoroutine = StartCoroutine(SteamEffectDuration());
        LogCurrentEffects(); // Steam efekti uyguland���nda logla
    }

    private IEnumerator SteamEffectDuration()
    {
        isSteamActive = true;
        LogCurrentEffects(); // Steam efekti aktif oldu�unda logla

        // Hareket h�z�n� yava�lat
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(speedMultiplier);
        }

        yield return new WaitForSeconds(steamDuration);

        // Hareket h�z�n� normale d�nd�r
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f);
        }

        isSteamActive = false;
        steamCoroutine = null;
        LogCurrentEffects(); // Steam efekti s�resi doldu�unda logla
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
        LogCurrentEffects(); // Steam efekti kald�r�ld���nda logla
    }

    // IDamagable aray�z�nden gelen Damage metodu
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
        // D��man�n yok edilmesi, skor artt�rma, animasyon oynatma gibi i�lemleri burada yapabilirsiniz
        Destroy(gameObject);
    }

    // Efektlerin durumlar�n� loglayan metot
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
