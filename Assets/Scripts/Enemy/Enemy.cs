using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BulletType enum�unun do�ru tan�mland���ndan emin olun


public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxHealth = 10f;
    private float currentHealth;
    public LayerMask enemyLayer;
    // Coroutine referanslar�
    private Coroutine fireCoroutine;
    private Coroutine waterCoroutine;
    private Coroutine electricCoroutine;
    private Coroutine steamCoroutine;

    // Fire efekti parametreleri
    private float fireDuration = 5f;
    private float fireDamagePerSecond = 1f;
    public GameObject firePrefab;

    // Water efekti parametreleri
    private float waterDuration = 5f;
    public GameObject waterPrefab;

    // Electric efekti parametreleri
    private float electricDuration = 5f;
    public GameObject electricPrefab;
    private float electricChainDamage = 3f;
    public GameObject electricChainPrefab;
    public int chainTimes = 1; // Zincirleme say�s�n� belirlemek i�in

    // Steam efekti parametreleri
    private float steamDuration = 8f;
    private float steamDamagePerSecond = 1f;
    public GameObject steamPrefab;
    private float speedReductionFactor = 0.8f; // %80 h�z azaltma
    public GameObject explosionPrefab;

    // Referanslar
    private EnemyMovement enemyMovement;

    // Efekt durum bayraklar�
    private bool isFireActive = false;
    private bool isWaterActive = false;
    private bool isElectricActive = false;
    private bool isSteamActive = false;


    // Explosion efekti parametreleri
    private float explosionDamage = 15f;
    private float explosionRadius = 5f;


    // Coroutine referans�
    private Coroutine waterElectricCoroutine;

    // Water-Electric efekt parametreleri
    [SerializeField] private int waterElectricDuration = 7; // Efektin s�resi (saniye)
    [SerializeField] private float waterElectricDamage = 3f; // Her saniye verilecek hasar miktar�
    [SerializeField] private float waterElectricRadius = 5f; // Zincir hasar�n�n uygulanaca�� yar��ap

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
            case BulletType.fire:
                ApplyFireEffect();
                break;
            case BulletType.water:
                ApplyWaterEffect();
                break;
            case BulletType.electric:
                ApplyElectricEffect();
                break;
            // Di�er mermi t�rlerini gerekti�inde ekleyebilirsiniz
            default:
                break;
        }

        // Merminin verdi�i hasar� uygula
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

            // Belirtilen yar��ap i�inde bulunan t�m d��manlar� bul
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, waterElectricRadius, enemyLayer);

            foreach (Collider2D enemyCollider in hitEnemies)
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>();
                GameObject electricChainEffect = Instantiate(electricChainPrefab, enemy.transform.position, Quaternion.identity);
                electricChainEffect.transform.parent = enemy.transform;
                if (enemy != null && enemy != this)
                {
                    Debug.Log($"{gameObject.name}'s Water-Electric effect deals {waterElectricDamage} damage to {enemy.gameObject.name}.");
                    enemy.Damage(waterElectricDamage);
                }
            }

            // 1 saniye bekle
            yield return new WaitForSeconds(1f);
        }

        // Coroutine tamamland���nda referans� s�f�rla
        waterElectricCoroutine = null;
    }
    private void ApplyExplosionEffect()
    {
        ClearEffects();
        // Patlama yar��ap� i�inde bulunan t�m d��manlar� bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
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

    // Fire efektini uygulayan metod
    private void ApplyFireEffect()
    {
        // E�er zaten fire efekti aktifse, mevcut coroutine'i durdur
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
        GameObject fireEffect = Instantiate(firePrefab, transform.position, Quaternion.identity);
        fireEffect.transform.parent = transform;

        float elapsed = 0f;

        while (elapsed < fireDuration)
        {
            TakeDamage(fireDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        isFireActive = false;
        Destroy(fireEffect);
        Debug.Log($"{gameObject.name} is no longer on fire.");
        fireCoroutine = null;
    }

    // Water efektini uygulayan metod
    private void ApplyWaterEffect()
    {
        // E�er zaten water efekti aktifse, mevcut coroutine'i durdur
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
        GameObject waterEffect = Instantiate(waterPrefab, transform.position, Quaternion.identity);
        waterEffect.transform.parent = transform;

        yield return new WaitForSeconds(waterDuration);

        isWaterActive = false;
        Destroy(waterEffect);
        Debug.Log($"{gameObject.name} is no longer affected by water.");
        waterCoroutine = null;
    }

    // Electric efektini uygulayan metod
    private void ApplyElectricEffect()
    {
        // E�er zaten electric efekti aktifse, mevcut coroutine'i durdur
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
        GameObject electricEffect = Instantiate(electricPrefab, transform.position, Quaternion.identity);
        electricEffect.transform.parent = transform;

        yield return new WaitForSeconds(electricDuration);
        
        isElectricActive = false;
        Destroy(electricEffect);
        Debug.Log($"{gameObject.name} is no longer electrified.");
        electricCoroutine = null;
    }

    private void ApplyElectricChainDamage()
    {
        // T�m Enemy objelerini bul
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        // Kendini hari� tut
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

        // Di�er d��manlar� mesafeye g�re s�ralay�n
        otherEnemies.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        // Zincirleme hasar verece�iniz d��man say�s�n� belirleyin
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
        // E�er zaten steam efekti aktifse, mevcut coroutine'i durdur
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
        GameObject steamEffect = Instantiate(steamPrefab, transform.position, Quaternion.identity);
        steamEffect.transform.parent = transform;

        ClearEffects();

        // Hareket h�z�n� azalt
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f - speedReductionFactor); // %80 azaltma i�in multiplier = 0.2
        }

        float elapsed = 0f;

        while (elapsed < steamDuration)
        {
            TakeDamage(steamDamagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        // Hareket h�z�n� eski haline getir
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeedMultiplier(1f); // Orijinal h�za geri d�n
            Debug.Log($"{gameObject.name}'s speed restored to original.");
        }

        isSteamActive = false;
        Destroy(steamEffect);
        Debug.Log($"{gameObject.name} is no longer affected by steam.");
        steamCoroutine = null;
    }

    // Efekt durumlar�n� loglayan metod (iste�e ba�l�)
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
