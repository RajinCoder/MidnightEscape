using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    int currentHealth;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            PlayerDies();
        }
        Debug.Log("Current health: " + currentHealth);
    }

    public void GainHealth(int healthAmount)
    {
        currentHealth += healthAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        healthSlider.value = currentHealth;
        Debug.Log("Current health: " + currentHealth);
    }

    void PlayerDies()
    {
        Debug.Log("Player is dead");
        FindObjectOfType<LevelManager>().LevelLost();
        transform.Rotate(-90, 0, 0, Space.Self);
    }
}
