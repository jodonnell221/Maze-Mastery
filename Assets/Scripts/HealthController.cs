using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthController : MonoBehaviour
{
    public Image healthBarImage; // Assign this in the Inspector
    public float health = 100f; //starting health
    private float maxHealth = 100f; //max health

    // Call this method to reduce health
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        UpdateHealthBar();
    }

    // Updates the visual representation of the health bar
    void UpdateHealthBar()
    {
        healthBarImage.fillAmount = health / maxHealth;
    }
}