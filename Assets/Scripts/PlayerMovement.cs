using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform orientation;
    public float moveSpeed;
    public float maxVelocity = 10;
    public float deceleration = 5; // Yavaþlama hýzý

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    Animator animator;

    public AudioSource footstepSound;

    public Player currPlayer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        footstepSound.volume = 0.07f;
    }

    // Update is called once per frame
    void Update()
    {
        if(currPlayer.isDead == false)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * moveSpeed * 5f * Time.deltaTime, ForceMode.Force);
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = moveDirection.normalized * maxVelocity;
            }

            // Update the walking animation state
            bool isWalking = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
            animator.SetBool("isWalking", isWalking);

            // Character deceleration when not moving
            if (!isWalking)
            {
                rb.velocity -= rb.velocity * deceleration * Time.deltaTime;
                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.velocity = Vector3.zero;
                }
            }

            if (isWalking)
            {
                if (footstepSound.enabled != true)
                {
                    footstepSound.enabled = true;
                }

            }
            else
            {
                footstepSound.enabled = false;
            }

            
        }
        else
        {
            footstepSound.enabled = false;
        }

       
    }
}