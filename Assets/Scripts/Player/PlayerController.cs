using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    public float jumpHeight = 1;
    public float gravity = 9.81f;
    public float airControl = 1;
    public AudioClip jumpSFX;
    public AudioClip walkSFX;
    public AudioClip heavyBreathingSFX;
    public AudioClip regularBreathingSFX;
    public float walkSFXDelay = .7f;


    CharacterController controller;
    Vector3 input, moveDirection;
    float normalSpeed, boostSpeed;
    float ogDelay;
    float runDelay;
    bool isPlayingWalkSFX = false;
    float timeRunning;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        normalSpeed = moveSpeed;
        boostSpeed = moveSpeed * 1.45f;
        ogDelay = walkSFXDelay;
        runDelay = walkSFXDelay - .2f;
        timeRunning = 0.0f;

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
            walkSFXDelay = runDelay;
            if (timeRunning > 3.0f && audioSource.clip != heavyBreathingSFX || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = heavyBreathingSFX;
                audioSource.Play();
            }
            timeRunning += Time.deltaTime;
        }
        else
        {
            moveSpeed = normalSpeed;
            walkSFXDelay = ogDelay;
            if (audioSource.clip != regularBreathingSFX || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = regularBreathingSFX;
                audioSource.Play();
            }
            timeRunning = 0.0f;

        }

        input *= moveSpeed;

        if ((moveHorizontal != 0 || moveVertical != 0) && !isPlayingWalkSFX)
        {
            StartCoroutine(PlayWalkSFX());
        }

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

    IEnumerator PlayWalkSFX()
    {
        isPlayingWalkSFX = true;
        AudioSource.PlayClipAtPoint(walkSFX, new Vector3(transform.position.x, 1, transform.position.z));
        yield return new WaitForSeconds(walkSFXDelay);
        isPlayingWalkSFX = false;
    }
}
