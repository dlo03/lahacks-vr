/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SampleUserPolling_ReadWrite : MonoBehaviour
{
    public SerialController serialController;
    public bool prev = false;
    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        Debug.Log("Press A or Z to execute some actions");
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------

        // If you press one of these keys send it to the serial device. A
        // sample serial device that accepts this input is given in the README.
                
        if (GetComponent<ObjectDetect>().triggerSignal != prev)
        {
            if (GetComponent<ObjectDetect>().triggerSignal == false)
            {
                Debug.Log("Sending Z");
                serialController.SendSerialMessage("Z");
            }
            else if (GetComponent<ObjectDetect>().triggerSignal == true)
            {
                Debug.Log("Sending A");
                serialController.SendSerialMessage("A");
            }
        }

        prev = GetComponent<ObjectDetect>().triggerSignal;

        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
        {
            return;
        }
        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        {
            Debug.Log("Connection established");
        }
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        {
            Debug.Log("Connection attempt failed or disconnection detected");
        }
        else
        {
            Debug.Log("Message arrived: " + message);
        }
    }
}
