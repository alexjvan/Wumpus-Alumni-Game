using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    public float enemyMaxHealth;
    public Slider healthBar;
    float currentHealth;
	// Use this for initialization
	void Start () {
        currentHealth = enemyMaxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(float damage)
    {
        currentHealth -= damage;

        healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
