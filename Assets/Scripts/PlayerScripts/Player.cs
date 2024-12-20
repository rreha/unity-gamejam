using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Ana menüye dönmek için gerekli
using TMPro; // Ekleme: TextMeshPro kullanýyorsanýz


public class Player : MonoBehaviour
{
    // Ekleme: UI elemanlarýný referans olarak ekleyin
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    [Header("Player Stats")]
    // Fire efekti parametreleri
    public float fireDuration = 5f;
    public float fireDamagePerSecond = 1f;

    // Water efekti parametreleri
    public float waterDuration = 5f;

    // Electric efekti parametreleri
    public float electricDuration = 5f;
    public float electricChainDamage = 3f;
    public float chainTimes = 1; // Zincirleme say?s?n? belirlemek i?in

    // Steam efekti parametreleri
    public float steamDuration = 8f;
    public float steamDamagePerSecond = 1f;
    public float speedReductionFactor = 0.8f; // %80 h?z azaltma
                                              // Explosion efekti parametreleri
    public float explosionDamage = 15f;
    public float explosionRadius = 5f;

    [SerializeField] public int waterElectricDuration = 7; // Efektin s?resi (saniye)
    [SerializeField] public float waterElectricDamage = 3f; // Her saniye verilecek hasar miktar?
    [SerializeField] public float waterElectricRadius = 5f; // Zincir hasar?n?n uygulanaca?? yar??ap

    [SerializeField] public float maxHp = 100f;
    [SerializeField] public float currentHp;

    // New stats
    [SerializeField] public float maxMana = 100f;
    [SerializeField] public float currentMana;
    [SerializeField] public float manaRegenRate = 5f; // Mana per second
    [SerializeField] public float attackSpeed = 1f; // Attacks per second





    private Coroutine damageCoroutine;


    private void Start()
    {

        currentMana = maxMana;
        currentHp = maxHp;

        UpdateUI();

    }
    // Ekleme: UI'yý güncelleyen metod
    public void UpdateUI()
    {
        healthText.text = $"HP: {currentHp}/{maxHp}";
        manaText.text = $"Mana: {currentMana}/{maxMana}";
    }
    // Düþmanla çarpýþma baþladýðýnda çaðrýlýr
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(TakeDamageOverTime(5f));
            }
        }
    }


    // Belirli bir saniye aralýðýnda sürekli hasar almayý saðlayan Coroutine
    private IEnumerator TakeDamageOverTime(float damagePerSecond)
    {
        while (true)
        {
            currentHp -= damagePerSecond;
            Debug.Log($"Player took {damagePerSecond} damage from Enemy collision. Current HP: {currentHp}");
            UpdateUI();
            if (currentHp <= 0)
            {
                Die();
                yield break; // Coroutine'i sonlandýr
            }

            yield return new WaitForSeconds(1f); // 1 saniye bekle
        }
    }

    // Oyuncu öldüðünde çaðrýlan metod
    private void Die()
    {
        Debug.Log("Player has died. Returning to Main Menu.");
        SceneManager.LoadScene("MainMenu"); // "MainMenu" sahnesinin doðru isimde olduðundan emin olun
    }

    // Düþmanla çarpýþma bittiðinde çaðrýlýr
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }




    public void IncreaseMaxHP(float hp)
    {
        maxHp += hp;
        currentHp += hp;
        Debug.Log($"Player health increased by {hp}. New Max HP: {maxHp}");
        UpdateUI();

    }
    public void IncreaseMaxMana(float mana)
    {
        maxMana += mana;
        Debug.Log($"Player mana increased by {mana}. New Max mana: {maxMana}");
        UpdateUI();

    }
    public void IncreaseManaRegenRate(float regen)
    {
        manaRegenRate += regen;
        Debug.Log($"Player mana regen rate increased by {regen}. regen rate: {manaRegenRate}");

    }

    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed += amount;

    }


    public void IncreaseFireDamage(float amount)
    {
        fireDamagePerSecond += amount;
    }

    public void IncreaseElectricDamage(float amount)
    {
        electricChainDamage += amount;
    }

    public void IncreaseExplosionDamage(float amount)
    {
        explosionDamage += amount;
    }
    public void IncreaseElectricChainAmount(float amount)
    {
        chainTimes += amount;
    }
    public void IncreaseSteamDamagePerSecond(float amount)
    {
        steamDamagePerSecond += amount;
    }
    public void IncreaseWaterElectricDamage(float amount)
    {
        waterElectricDamage += amount;
    }
    public void RadiusIncrease(float amount)
    {
        explosionRadius += amount;
        waterElectricRadius += amount;
    }





}
