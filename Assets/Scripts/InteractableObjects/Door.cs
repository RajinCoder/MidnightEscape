using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float rotationAmount = 90f;
    public float rotationSpeed = 2f;
    private Quaternion initRotation;

    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != initRotation) {
            transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, Time.deltaTime * rotationSpeed);
            }
    }

    public void Open()
    {
        if (!isOpen)
        {
            initRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * rotationAmount);
            isOpen = true;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            initRotation = Quaternion.Euler(transform.eulerAngles - Vector3.up * rotationAmount);
            isOpen = false;
        }
    }
}
