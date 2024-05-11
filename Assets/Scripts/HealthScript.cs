using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;
    int health;
    bool dead;

    void Start()
    {
        dead = false;
        health = maxHealth;
        if(healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(healthBar)
        {
            healthBar.value = health;
        }
        if(health <= 0) 
        {
            dead = true;
        }
    }

    public bool IsDead()
    {
        return dead;
    }
}
