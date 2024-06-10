using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount) {
        if (currentHealth > 0) {
            currentHealth -= damageAmount;
        } 
        if (currentHealth <= 0) {
            PlayerDies();
        }
        Debug.Log("Current health: " + currentHealth);
    }

    void PlayerDies() {
        Debug.Log("Player is dead");
        FindObjectOfType<LevelManager>().LevelLost();
        transform.Rotate(-90, 0, 0, Space.Self);
    }
}
