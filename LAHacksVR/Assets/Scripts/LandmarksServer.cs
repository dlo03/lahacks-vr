using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp; // for web server connection
using Newtonsoft.Json;

public class LandmarksServer : MonoBehaviour
{
    WebSocket socket;
    public MoveCoordinate[] nodes;

    // Start is called before the first frame update

    void getNodes()
    {
        nodes = GetComponents<MoveCoordinate>();
    }

    void Start()
    {
        getNodes();

        socket = new WebSocket("ws://localhost:3030");

        socket.OnOpen += (sender, e) =>
        {
            Debug.Log("Connection established");
            socket.Send("Connection established in Unity");
        };

        socket.OnMessage += (sender, e) =>
        {
            //Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data: " + e.Data);
            // Using Newtonsoft.Json
            var landmarks = JsonConvert.DeserializeObject<Dictionary<string, float>[]>(e.Data);

            foreach (MoveCoordinate n in nodes)
            {
                n.UpdateLandmark(landmarks);
            }
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
