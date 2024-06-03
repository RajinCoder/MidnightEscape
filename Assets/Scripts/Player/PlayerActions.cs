using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    private Transform Camera;

    private float maxUseDistance = 5f;
    public AudioClip doorOpenSFX;
    public AudioClip doorCloseSFX;

    // private LayerMask UseLayers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DoorAction();
    }

    private void FixedUpdate()
    {

    }

    void DoorAction()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxUseDistance) &&
            hit.collider.CompareTag("Door"))
        {
            Debug.Log("In front of Door");
            if (hit.collider.TryGetComponent(out Door door))
            {
                Debug.Log("Get Component Door");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Open/Close Door");
                    if (!door.isOpen)
                    {
                        AudioSource.PlayClipAtPoint(doorOpenSFX, transform.position);
                        door.Open();
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(doorCloseSFX, transform.position);
                        door.Close();
                    }
                }
            }
        }
    }
}