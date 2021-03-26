using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp; // for web server connection
using Newtonsoft.Json;

public class LandmarksServer : MonoBehaviour
{
    public Vector3 target;
    WebSocket socket;
    //Dictionary<string, float>[] landmarks;


    // Start is called before the first frame update
    void Start()
    {
        socket = new WebSocket("ws://localhost:3030");

        socket.OnOpen += (sender, e) =>
        {
            Debug.Log("Connection established");
            socket.Send("Connection established in Unity");
        };

        socket.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data: " + e.Data);
            // Using Newtonsoft.Json
            Debug.Log("Hi");
            //landmarks = JsonConvert.DeserializeObject<Dictionary<string, float>[]>(e.Data);
            var landmarks = JsonConvert.DeserializeObject<Dictionary<string, float>[]>(e.Data);
            Debug.Log("Landmarks here:");
            Debug.Log(landmarks);
            //int x = int.Parse(landmarks["x"]);
            //int y = int.Parse(landmarks["y"]);
            //int z = int.Parse(landmarks["z"]);
            //target = new Vector3(x, y, z);
            BroadcastMessage("UpdateLandmark", landmarks);
            
        };

        socket.OnClose += (sender, e) =>
        {
            Debug.Log("Connection closed with " + e.Reason);
        };

        socket.Connect();
    }

    void OnApplicationQuit()
    {
        if (socket != null)
        {
            socket.Close(1001, "Game Quit");
        }
    }
}
