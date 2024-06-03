using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10;
    public float walkDistance = 10f;
    public float runDistance = 5f;
    public int damageAmount = 20;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > walkDistance)
        {
            animator.SetInteger("animState", 0); // Idle
        }
        else if (distance > runDistance)
        {
            animator.SetInteger("animState", 1); // Walking
        }
        else
        {
            animator.SetInteger("animState", 2); // Running
        }

        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }

    public void Die()
    {
        animator.SetInteger("animState", 3); // Dead
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
