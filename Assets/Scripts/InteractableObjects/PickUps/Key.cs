using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [Header("Key Settings")]
    public int keyIdentity;
    public Text textUI;
    public GameObject objToActivate;

    public GameObject obj;
    bool inReach;

    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {
            textUI.text = "";
            textUI.gameObject.SetActive(false);
            objToActivate.SetActive(true);
            obj.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            textUI.text = "PICK UP";
            textUI.gameObject.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            textUI.text = "";
            textUI.gameObject.SetActive(false);
        }
    }
}
