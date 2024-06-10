using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public GameObject burgerPrefab;
    public Transform spawnPoint;
    public Transform player;
    public float maxDistance = 5f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && IsPlayerWithinDistance())
        {
            DropBurger();
        }
    }

    void DropBurger()
    {
        if (burgerPrefab != null && spawnPoint != null)
        {
            Instantiate(burgerPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Burger dropped.");
        }
        else
        {
            Debug.LogError("Burger prefab or spawn point is not set.");
        }
    }

    bool IsPlayerWithinDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            return distance <= maxDistance;
        }
        return false;
    }
}
