using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    public float jumpHeight = 1;
    public float gravity = 9.81f;
    public float airControl = 1;

    CharacterController controller;
    Vector3 input, moveDirection;
    float normalSpeed, boostSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        normalSpeed = moveSpeed;
        boostSpeed = moveSpeed * 1.45f;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        // Sprint/Boost Control
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = boostSpeed;
        }
        else
        {
            moveSpeed = normalSpeed;
        }

        input *= moveSpeed;

        // Jump Control
        if (controller.isGrounded)
        {
            moveDirection = input;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            // Midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
