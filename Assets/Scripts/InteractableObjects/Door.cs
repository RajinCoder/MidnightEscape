using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float rotationAmount = 90f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        if (!isOpen)
        {
            transform.Rotate(Vector3.up * rotationAmount);
            isOpen = true;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            transform.Rotate(Vector3.up * rotationAmount * -1);
            isOpen = false;
        }
    }
}
