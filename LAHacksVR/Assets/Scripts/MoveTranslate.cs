using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTranslate : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Input.GetKey("w"))
        //{
        //    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("s"))
        //{
        //    transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("a"))
        //{
        //    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("d"))
        //{
        //    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("space"))
        //{
        //    transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
        //}
        //if (Input.GetKey("left shift"))
        //{
        //    transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
        //}

        //Store user input as a movement vector
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        //m_Input = m_Input.normalized * moveSpeed * Time.deltaTime;

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        rb.MovePosition(transform.position + m_Input);
    }
}
