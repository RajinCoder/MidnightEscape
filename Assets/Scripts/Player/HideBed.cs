using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBed : MonoBehaviour
{
    public float rayLength = 10f;

    Camera hidingCamera;
    bool isHiding = false;
    bool guiShow = false;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Hide") && !isHiding)
            {
                guiShow = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Disable Player
                    gameObject.GetComponent<MeshRenderer>().enabled = false;

                    // Change Camera
                    hidingCamera = hit.collider.GetComponentInChildren<Camera>();
                    hidingCamera.enabled = true;

                    audioSource.volume = .5f;

                    StartCoroutine(Wait());
                }
            }
        }
        else
        {
            guiShow = false;
        }

        if (isHiding == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Enable Player
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                // Change Camera
                hidingCamera.enabled = false;

                audioSource.volume = .8f;

                isHiding = false;
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        isHiding = true;
        guiShow = false;
    }

    private void OnGUI()
    {
        if (guiShow == true)
        {
            GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 12.5f, 100, 25), "Hide Inside?");
        }
    }
}