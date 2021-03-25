using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveForce = 2000f;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Fixed Update is used since it's best for Physics type stuff
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(0, 0, moveForce * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(0, 0, -1 * moveForce * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(-1 * moveForce * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(moveForce * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("space"))
        {
            rb.AddForce(0, moveForce * Time.deltaTime, 0);
        }
        if (Input.GetKey("left shift"))
        {
            rb.AddForce(0, -1 * moveForce * Time.deltaTime, 0);
        }
    }
}
