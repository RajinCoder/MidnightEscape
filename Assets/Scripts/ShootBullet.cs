using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPrefab,
                transform.position + transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);

            bullet.transform.SetParent(
                GameObject.FindGameObjectWithTag("BulletParent").transform);
        }
    }
}

