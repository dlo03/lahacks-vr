using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //TODO:
    // If you are holding down the key and then move to an object, it grabs it. Fix this. Think about KeyUp? KeyDown? Instead of Input.getKey
    // Not make the object warp?
    // Check for multiple objects? (Do later)
    // You can pick up from anywhere
    // When you release an object, the physics immediately comes back and knocks the hand to downtown LA. Solved: Set to kinematic

    public float throwForce = 600;
    Vector3 objectPos;

    public bool holdable = true;
    public bool holding = false;
    public GameObject hand;

    private Rigidbody rigidBody;
    private GameObject handModel;
    private Collider collider;


    void Start()
    {
        hand = GameObject.Find("/Hand");
        handModel = GameObject.Find("/Hand/Hand");
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

    }

    void Update()
    {
        // If the object is being held and it is being actively grabbed (p is pressed), grab it
        if (holding == true && Input.GetKey("p"))
        {
            // To ensure that collisions are able to be acheived while holding the object
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.isKinematic = true;

            // Set the item to be the child of the hand
            this.transform.SetParent(hand.transform);

            // Disable collider since without it, the hande jiggles around
            //collider.enabled = false;


            // Move the object to the palm
            //this.transform.localPosition = new Vector3(0.0224f, 0.0343f, -0.0742f);
            this.transform.localPosition = Vector3.zero;

            //gameObject.layer = 9;
            //Physics.IgnoreLayerCollision(8, 9, true);

        } else
        {
            holding = false;
            rigidBody.isKinematic = false;

            //gameObject.layer = 0;
            //Physics.IgnoreLayerCollision(8, 9, false);

            // Reenable collisions
            //collider.enabled = true;
            rigidBody.isKinematic = false;
            objectPos = this.transform.position;
            transform.SetParent(null);
            rigidBody.useGravity = true;
            this.transform.position = objectPos;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check to see if the hand is grasping
        //hand = GameObject.Find("Hand/Hand");
        //Debug.Log(hand.GetComponent<HandAnimator>().graspValue);
        if (handModel.GetComponent<HandAnimator>().graspValue > 0.5f && collision.gameObject.name == "Hand")
        {
            //Debug.Log("Trigger");
            holding = true;
            rigidBody.useGravity = false;
            rigidBody.detectCollisions = true;

        } else
        {
            holding = false;
        }
    }

}
