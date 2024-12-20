using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static BulletBehavior;

public class PlayerAimAndShoot : MonoBehaviour
{
    [Header("PlayerStats")]
    public Player playerStats;



    [Header("AimAndShoot")]
    public GameObject firebullet;
    public GameObject electricbullet;
    public GameObject waterbullet;


    private GameObject bulletInst;
    public Transform bulletSpawnPoint;
    public Camera mainCamera;
    public Transform weaponPivot;
    BulletType bulletType = BulletType.fire;

    // Attack speed management
    private float attackCooldown = 0f;

    // Mana cost per bullet type
    public float fireManaCost = 7;
    public float electricManaCost = 7;
    public float waterManaCost = 7;

    private void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<Player>();
            if (playerStats == null)
            {
                Debug.LogError("Player instance not found in the scene.");
            }
        }
    }

    private void HandleGunShooting()
    {
        // Check if the player can attack based on attack speed
        if (attackCooldown > 0f)
            return;

        if (Input.GetMouseButton(0))
        {
            // Determine mana cost based on bullet type
            float manaCost = 0f;
            switch (bulletType)
            {
                case BulletType.fire:
                    manaCost = fireManaCost;
                    break;
                case BulletType.electric:
                    manaCost = electricManaCost;
                    break;
                case BulletType.water:
                    manaCost = waterManaCost;
                    break;
            }

            // Check if the player has enough mana
            if (playerStats.currentMana < manaCost)
            {
                Debug.Log("Not enough mana to shoot.");
                return;
            }

            // Deduct mana
            playerStats.currentMana -= manaCost;
            playerStats.UpdateUI();

            // Spawn bullet based on type
            switch (bulletType)
            {
                case BulletType.fire:
                    Debug.Log("Fire shot fired.");
                    bulletInst = Instantiate(firebullet, bulletSpawnPoint.position, weaponPivot.rotation);
                    break;
                case BulletType.electric:
                    Debug.Log("Electric shot fired.");
                    bulletInst = Instantiate(electricbullet, bulletSpawnPoint.position, weaponPivot.rotation);
                    break;
                case BulletType.water:
                    Debug.Log("Water shot fired.");
                    bulletInst = Instantiate(waterbullet, bulletSpawnPoint.position, weaponPivot.rotation);
                    break;
            }

            // Reset attack cooldown
            attackCooldown = 1f / playerStats.attackSpeed;
        }
    }
    void Update()
    {
        // Handle attack cooldown
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Handle mana regeneration
        RegenerateMana();

        // Handle bullet type switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bulletType = BulletType.fire;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bulletType = BulletType.electric;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bulletType = BulletType.water;
        }

        HandleGunShooting();
        Aim();

        // Optional: Debug logs for mana and attack speed
        // Debug.Log($"Mana: {playerStats.currentMana}/{playerStats.maxMana}");
        // Debug.Log($"Attack Cooldown: {attackCooldown}");
    }

    private void RegenerateMana()
    {
        if (playerStats.currentMana < playerStats.maxMana)
        {
            playerStats.currentMana += playerStats.manaRegenRate * Time.deltaTime;
            playerStats.currentMana = Mathf.Min(playerStats.currentMana, playerStats.maxMana);
        }
        playerStats.UpdateUI();
    }
    private void Aim()
    {
        // Mouse pozisyonunu dünya pozisyonuna çevir
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Z eksenini sıfırla (2D için)
        mousePosition.z = 0f;

        // Yön vektörünü hesapla
        Vector3 direction = mousePosition - weaponPivot.position;

        // Rotasyonu hesapla
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Silah pivotunu döndür
        weaponPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }





}
