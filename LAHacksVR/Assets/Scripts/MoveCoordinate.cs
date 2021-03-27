using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCoordinate : MonoBehaviour
{
    // Start is called before the first frame update
    float t = 0f;
    public float moveSpeed = 5f;
    public Vector3 target;
    public Vector3 target_rotation;

    private Rigidbody rb;
    private float lastSqrMag;
    private Vector3 directionalVector;

    Vector3 m_EulerAngleVelocity;

    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody>();

        // Initiate previous square distance
        lastSqrMag = Mathf.Infinity;

        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        m_EulerAngleVelocity = new Vector3(0, 100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Listen to Web server and obtain the target, set the target to be the vector
        // Think about increasing the move speed probably?

        // Calculate the directional vector from the current position to a target position
        directionalVector = (target - transform.position).normalized * moveSpeed;
        //Debug.Log(directionalVector);
        
        // Determine the square distance to the target
        float sqrMag = (target - transform.position).sqrMagnitude;

        // If you are past the target, stop all movement
        if (sqrMag > lastSqrMag)
        {
            moveSpeed = 0f;
        }

        // Set previous square distance
        lastSqrMag = sqrMag;
    }

    void FixedUpdate()
    {
        // Movement should be done in a Fixed Update
        rb.MovePosition(transform.position + directionalVector * Time.deltaTime);

        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
