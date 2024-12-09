using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
    public GameObject bullet;
    private	 GameObject bulletInst;
    public Transform bulletSpawnPoint;
    public Camera mainCamera;
    public Transform weaponPivot;
 private void HandleGunShooting()
 {
    if(Mouse.current.leftButton.wasPressedThisFrame)
    {
      //spawn bullet
      bulletInst = Instantiate(bullet, bulletSpawnPoint.position, weaponPivot.rotation);
    }
 }
  void Update()
 {
    HandleGunShooting();
    Aim();
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
