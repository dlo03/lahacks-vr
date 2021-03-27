using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetect : MonoBehaviour
{
    //TODO:
    // There is a non-breaking issue where when you drop an object after grasping, it tries to add the same object to the dictionary. Likely because we're disabling the collision?
    // However, the removal of all objects works fine.
    public bool triggerSignal;
    private Dictionary<string, bool> objDictionary = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a pickable object in the spherical collider, 
    }

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Collision detected");
        if (col.gameObject.tag == "PickableObject")
        {
            if (objDictionary.Count == 0)
            {
                Debug.Log("First object in trigger");
                triggerSignal = true;
            }
            objDictionary.Add(col.gameObject.name, true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        //Debug.Log("Collision off");
        if (col.gameObject.tag == "PickableObject")
        {
            // Remove from dictionary
            objDictionary.Remove(col.gameObject.name);

            if (objDictionary.Count == 0)
            {
                Debug.Log("No Objects in trigger");
                triggerSignal = false;
            }
            //Debug.Log("Off Trigger");
        }
    }
}
