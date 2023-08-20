using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public Transform orientation;
    public float moveSpeed;
    public float maxVelocity = 10;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;


    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical"); 

        moveDirection =  orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * Time.deltaTime, ForceMode.Force);
        if(rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = moveDirection.normalized * maxVelocity;
        }
    }
}
