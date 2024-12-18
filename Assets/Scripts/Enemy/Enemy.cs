using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BulletType enum’unun doðru tanýmlandýðýndan emin olun


public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxHealth = 10f;
    private float currentHealth;
    public LayerMask enemyLayer;
    // Coroutine referanslarý
    private Coroutine fireCoroutine;
    private Coroutine waterCoroutine;
    private Coroutine electricCoroutine;
    private Coroutine steamCoroutine;

    // Fire efekti parametreleri
    private float fireDuration = 5f;
    private float fireDamagePerSecond = 1f;

    // Water efekti parametreleri
    private float waterDuration = 5f;

    // Electric efekti parametreleri
    private float electricDuration = 5f;
    private float electricChainDamage = 3f;
    public int chainTimes = 1; // Zincirleme sayýsýný belirlemek için

    // Steam efekti parametreleri
    private float steamDuration = 8f;
    private float steamDamagePerSecond = 1f;
    private float speedReductionFactor = 0.8f; // %80 hýz azaltma

    // Referanslar
    private EnemyMovement enemyMovement;

    // Efekt durum bayraklarý
    private bool isFireActive = false;
    private bool isWaterActive = false;
    private bool isElectricActive = false;
    private bool isSteamActive = false;


    // Explosion efekti parametreleri
    private float explosionDamage = 15f;
    private float explosionRadius = 5f;


    // Coroutine referansý
    private Coroutine waterElectricCoroutine;

    // Water-Electric efekt parametreleri
    [SerializeField] private int waterElectricDuration = 7; // Efektin süresi (saniye)
    [SerializeField] private float waterElectricDamage = 3f; // Her saniye verilecek hasar miktarý
    [SerializeField] private float waterElectricRadius = 5f; // Zincir hasarýnýn uygulanacaðý yarýçap

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
            case BulletType.fire:
                ApplyFireEffect();
                break;
            case BulletType.water:
                ApplyWaterEffect();
                break;
            case BulletType.electric:
                ApplyElectricEffect();
                break;
            // Diðer mermi türlerini gerektiðinde ekleyebilirsiniz
            default:
                break;
        }

        // Merminin verdiði hasarý uygula
        Damage(damageAmount);

        // Fire ve Water etkileri aktifse Steam efektini uygula
        if (isFireActive && isWaterActive && !isSteamActive)
        {
            ApplySteamEffect();
        }
        // Fire ve Electric etkileri aktifse Explosion efektini uygula
        if (isFireActive && isElectricActive)
        {
            ApplyExplosionEffect();
        }

        // Su ve Elektrik etkileri aktifse Water-Electric efektini uygula
        if (isWaterActive && isElectricActive && waterElectricCoroutine == null)
        {
            waterElectricCoroutine = StartCoroutine(WaterElectricEffectCoroutine());
        }
    }

    private IEnumerator WaterElectricEffectCoroutine()
    {
        Debug.Log($"{gameObject.name} is affected by the Water-Electric combination!");
        ClearEffects();
        for (int i = 0; i < waterElectricDuration; i++)
        {
            // Kendisine hasar uygula
            Debug.Log($"{gameObject.name} receives {waterElectricDamage} chain damage due to Water-Electric effect.");
            TakeDamage(waterElectricDamage);

            // Belirtilen yarýçap içinde bulunan tüm düþmanlarý bul
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, waterElectricRadius, enemyLayer);

            foreach (Collider2D enemyCollider in hitEnemies)
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>();
                if (enemy != null && enemy != this)
                {
                    Debug.Log($"{gameObject.name}'s Water-Electric effect deals {waterElectricDamage} damage to {enemy.gameObject.name}.");
                    enemy.Damage(waterElectricDamage);
                }
            }

            // 1 saniye bekle
            yield return new WaitForSeconds(1f);
        }

        // Coroutine tamamlandýðýnda referansý sýfýrla
        waterElectricCoroutine = null;
    }
    private void ApplyExplosionEffect()
    {
        ClearEffects();
        // Patlama yarýçapý içinde bulunan tüm düþmanlarý bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null && enemy != this)
            {
                Debug.Log($"{gameObject.name}'s explosion deals {explosionDamage} damage to {enemy.gameObject.name}.");
                enemy.Damage(explosionDamage);
            }
        }
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

    // Fire efektini uygulayan metod
    private void ApplyFireEffect()
    {
        // Eðer zaten fire efekti aktifse, mevcut coroutine'i durdur
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }

        fireCoroutine = StartCoroutine(FireEffectCoroutine());
    }

    private IEnumerator FireEffectCoroutine()
    {
        isFireActive = true;
        Debug.Log($"{gameObject.name} is on fire!");

        float elapsed = 0f;

        while (elapsed < fireDuration)
        {
            TakeDamage(fireDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        isFireActive = false;
        Debug.Log($"{gameObject.name} is no longer on fire.");
        fireCoroutine = null;
    }

    // Water efektini uygulayan metod
    private void ApplyWaterEffect()
    {
        // Eðer zaten water efekti aktifse, mevcut coroutine'i durdur
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
        }

        waterCoroutine = StartCoroutine(WaterEffectCoroutine());
    }

    private IEnumerator WaterEffectCoroutine()
    {
        isWaterActive = true;
        Debug.Log($"{gameObject.name} is affected by water!");

        yield return new WaitForSeconds(waterDuration);

        isWaterActive = false;
        Debug.Log($"{gameObject.name} is no longer affected by water.");
        waterCoroutine = null;
    }

    // Electric efektini uygulayan metod
    private void ApplyElectricEffect()
    {
        // Eðer zaten electric efekti aktifse, mevcut coroutine'i durdur
        if (electricCoroutine != null)
        {
            StopCoroutine(electricCoroutine);
        }
        ApplyElectricChainDamage();
        electricCoroutine = StartCoroutine(ElectricEffectCoroutine());

       
    }

    private IEnumerator ElectricEffectCoroutine()
    {
        isElectricActive = true;
        Debug.Log($"{gameObject.name} is electrified!");

        yield return new WaitForSeconds(electricDuration);

        isElectricActive = false;
        Debug.Log($"{gameObject.name} is no longer electrified.");
        electricCoroutine = null;
    }

    private void ApplyElectricChainDamage()
    {
        // Tüm Enemy objelerini bul
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        // Kendini hariç tut
        List<Enemy> otherEnemies = new List<Enemy>();
        foreach (var enemy in allEnemies)
        {
            if (enemy != this)
            {
                otherEnemies.Add(enemy);
            }
        }

        if (otherEnemies.Count == 0)
        {
            Debug.Log("No other enemies found to apply electric chain damage.");
            return;
        }

        // Diðer düþmanlarý mesafeye göre sýralayýn
        otherEnemies.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        // Zincirleme hasar vereceðiniz düþman sayýsýný belirleyin
        int targets = Mathf.Min(chainTimes, otherEnemies.Count);

        for (int i = 0; i < targets; i++)
        {
            Enemy targetEnemy = otherEnemies[i];
            Debug.Log($"{gameObject.name} electrified {targetEnemy.gameObject.name} for {electricChainDamage} damage.");
            targetEnemy.Damage(electricChainDamage);
        }
    }

    // Steam efektini uygulayan metod
    private void ApplySteamEffect()
    {
        // Eðer zaten steam efekti aktifse, mevcut coroutine'i durdur
        if (steamCoroutine != null)
        {
            StopCoroutine(steamCoroutine);
        }

        steamCoroutine = StartCoroutine(SteamEffectCoroutine());
    }
    private void ClearEffects()
    {
        // Mevcut Fire ve Water ve electric efektlerini durdur
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
            waterCoroutine = null;
        }
        if (electricCoroutine != null)
        {
            StopCoroutine(electricCoroutine);
            electricCoroutine = null;
        }
        isFireActive = false;
        isWaterActive = false;
        isElectricActive = false;

    }
    private IEnumerator SteamEffectCoroutine()
    {
        isSteamActive = true;
        Debug.Log($"{gameObject.name} is affected by steam!");

       ClearEffects();

        // Hareket hýzýný azalt
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f - speedReductionFactor); // %80 azaltma için multiplier = 0.2
        }

        float elapsed = 0f;

        while (elapsed < steamDuration)
        {
            TakeDamage(steamDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        // Hareket hýzýný eski haline getir
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f); // Orijinal hýza geri dön
            Debug.Log($"{gameObject.name}'s speed restored to original.");
        }

        isSteamActive = false;
        Debug.Log($"{gameObject.name} is no longer affected by steam.");
        steamCoroutine = null;
    }

    // Efekt durumlarýný loglayan metod (isteðe baðlý)
    private void LogCurrentEffects()
    {
        Debug.Log($"Effects on {gameObject.name} - Fire: {isFireActive}, Water: {isWaterActive}, Electric: {isElectricActive}, Steam: {isSteamActive}");
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
