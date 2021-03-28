using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCoordinate : MonoBehaviour
{
    // Start is called before the first frame update
    public bool rotation = false;

    public float moveSpeed = 5f;
    public Vector3 target;
    public Vector3 target_rotation;
    public int id = 1; // defaults to 1 (the wrist)

    private Rigidbody rb;
    private float lastSqrMag;
    private Vector3 directionalVector;
    
    private Vector3 thresholdVec = new Vector3(0.1f, 0.1f, 0.1f);
    private float thresholdMag;
    private float sqrMag;

    private float rotateThresh = 25;
    private float rotateCap = 360;
    private Vector3 angleThreshVec = new Vector3(5, 5, 15);


    private Dictionary<string, float>[] previousLandmarks;

    Vector3 m_EulerAngleVelocity;

    public void UpdateLandmark(Dictionary<string, float>[] landmarks)
    {
        Debug.Log("Landmark updated.");
        var coords = landmarks[id];
        target = new Vector3(-4 + coords["x"] * 8, (float)1.35 + coords["z"] * 10, -2 + coords["y"] * 8);

        if (previousLandmarks != null)
        {
            // Pinky
            Dictionary<string, float> pinkyAnchor = landmarks[19];
            Dictionary<string, float> prevPinky = previousLandmarks[19];

            // Thumb
            Dictionary<string, float> thumbAnchor = landmarks[3];
            Dictionary<string, float> prevThumb = previousLandmarks[3];


            // Wrist?
            Dictionary<string, float> wristAnchor = landmarks[1];
            Dictionary<string, float> prevWrist = previousLandmarks[1];


            Vector3 thumbVec = new Vector3(thumbAnchor["x"] - wristAnchor["x"], thumbAnchor["z"] - wristAnchor["z"], thumbAnchor["y"] - wristAnchor["y"]) * 1000;
            Vector3 prevThumbVec = new Vector3(prevThumb["x"] - prevWrist["x"], prevThumb["z"] - prevWrist["z"], prevThumb["y"] - prevWrist["y"]) * 1000;

            Vector3 pinkyVec = new Vector3(pinkyAnchor["x"] - wristAnchor["x"], pinkyAnchor["z"] - wristAnchor["z"], pinkyAnchor["y"] - wristAnchor["y"]) * 1000;
            Vector3 prevPinkyVec = new Vector3(prevPinky["x"] - prevWrist["x"], prevPinky["z"] - prevWrist["z"], prevPinky["y"] - prevWrist["y"]) * 1000;

            float angleThumb = Vector3.Angle(thumbVec, prevThumbVec);
            float anglePinky = Vector3.Angle(pinkyVec, prevPinkyVec);

            Vector3 oldNorm = Vector3.Cross(prevThumbVec, prevPinkyVec);
            Vector3 newNorm = Vector3.Cross(thumbVec, pinkyVec);

            //Debug.Log("Old Thumb: " + prevThumbVec.ToString() + "Old Pinky: " + prevPinkyVec.ToString());
            //Debug.Log("Thumb: " + thumbVec.ToString() + " Pinky: " + pinkyVec.ToString());


            //Debug.Log("Norm Values: " + oldNorm.ToString() + ", " + newNorm.ToString());

            //Debug.Log("Norm angles: " + Vector3.Angle(oldNorm, newNorm).ToString());

            //if ((angleThumb < 0 && anglePinky > 0) || (angleThumb > 0 && anglePinky < 0))
            //{
            //    target_rotation = new Vector3(0, 0, -1 * Vector3.Angle(oldNorm, newNorm)); // when hand is reflected
            //} else
            //{
            //    target_rotation = new Vector3(0, 0, Vector3.Angle(oldNorm, newNorm));
            //}

            target_rotation = new Vector3(0, 0, Vector3.Angle(oldNorm, newNorm));

            Debug.Log("Target Rotation: " + target_rotation.ToString());
        }


        previousLandmarks = landmarks;
    }

    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody>();

        // Initiate previous square distance
        lastSqrMag = Mathf.Infinity;

        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        m_EulerAngleVelocity = target_rotation;
        thresholdMag = thresholdVec.sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        // Listen to Web server and obtain the target, set the target to be the vector
        // Think about increasing the move speed probably?

        // Calculate the directional vector from the current position to a target position
        directionalVector = (target - transform.position).normalized * moveSpeed;
        //Debug.Log("Directional Vector");
        //Debug.Log(directionalVector);

        // Determine the square distance to the target
        sqrMag = (target - transform.position).sqrMagnitude;

        // Scale movespeed accordingly to target distance
        moveSpeed = sqrMag;

        // If you are past the target or the magnitude is less than a threshold magnitude, stop all movement
        if (sqrMag > lastSqrMag || sqrMag <= thresholdMag)
        {
            moveSpeed = 0f;
        }

        // Set previous square distance
        lastSqrMag = sqrMag;
    }

    void FixedUpdate()
    {
        // Movement should be done in a Fixed Update
        // First if: to prevent oscillations
        // Second if: A threshold/range to move, which is if the difference is the magnitude
        if (directionalVector != -1 * directionalVector && sqrMag >= thresholdMag)
        {
            rb.MovePosition(transform.position + directionalVector * Time.deltaTime);
        }

        //Debug.Log("Angle Dist: " + Vector3.Distance(transform.eulerAngles.normalized, target_rotation.normalized).ToString());

        // transform.eulerAngles += target_rotation;

        Debug.Log("SqrMag: " + target_rotation.sqrMagnitude);

        if (rotation && target_rotation.sqrMagnitude > rotateThresh)
        {
            transform.eulerAngles += target_rotation;
            target_rotation = new Vector3(0, 0, 0);
        }



        // transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, target_rotation, Time.deltaTime);

        // Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        //rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
