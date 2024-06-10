using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : MonoBehaviour
{
    public int healthAmount = 20; 
    public float interactDistance = 3f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null && Vector3.Distance(player.transform.position, transform.position) <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.GainHealth(healthAmount);
                    Destroy(gameObject);
                }
            }
        }
    }
}
