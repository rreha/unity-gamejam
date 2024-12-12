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

        if(currentHealth<=0 )
        {
            Destroy(gameObject);
        }
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
