using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField]  public float maxHealth = 10f;
    [SerializeField] private float currentHealth;
    
    void IDamagable.Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Enemy damaged! Current health: {currentHealth}");

        if(currentHealth<=0 )
        {
           StartCoroutine(Respawn());
           Destroy(gameObject);
        }
    }

    private IEnumerator Respawn()
{
    gameObject.SetActive(false); // Deactivate enemy instead of destroying it
    yield return new WaitForSeconds(5f); // Wait for 5 seconds before respawning
    
    currentHealth = maxHealth; // Reset health
    gameObject.SetActive(true); // Reactivate enemy
}

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
