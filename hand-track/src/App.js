import React, { useRef } from "react";
import Webcam from "react-webcam";
import './App.css';

function App() {
  // Web Server
  const CONNECT_PORT = 3030;
  let socket;
  var connectionEstablished = false;


  const connect = (connectPort) => {
    socket = new WebSocket(`ws://localhost:${connectPort}`);

    socket.addEventListener("open", (event) => {
      socket.send("Connection established.");
      console.log(`Connection to server at port ${connectPort} established`);
      connectionEstablished = true;
    });

    socket.addEventListener("close", (event) => {
      console.log(`Connection closed at port ${connectPort}`);
      connectionEstablished = false;
    });
  };

  connect(CONNECT_PORT);

  

  // Webcam and Mediapipe Processes

  const CAM_WIDTH = 1280;
  const CAM_HEIGHT = 720;

  const webcamRef = useRef(null);
  const canvasRef = useRef(null);

  /**
   * @license
   * Copyright 2021 Google LLC. All Rights Reserved.
   * Licensed under the Apache License, Version 2.0 (the "License");
   * you may not use this file except in compliance with the License.
   * You may obtain a copy of the License at
   *
   * https://www.apache.org/licenses/LICENSE-2.0
   *
   * Unless required by applicable law or agreed to in writing, software
   * distributed under the License is distributed on an "AS IS" BASIS,
   * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   * See the License for the specific language governing permissions and
   * limitations under the License.
   * =============================================================================
   */

  const onResult = (results) => {
    const ctx = canvasRef.current.getContext("2d");
    const canvHeight = canvasRef.current.height;
    const canvWidth = canvasRef.current.width;

    ctx.save();
    ctx.clearRect(0, 0, canvWidth, canvHeight);
    ctx.drawImage(
      results.image, 0, 0, canvWidth, canvHeight);

    if (results.multiHandLandmarks) {
      for (let [key, landmarks] of Object.entries(results.multiHandLandmarks)) {
        console.log(landmarks);
        var classification = results.multiHandedness[key].label;
        landmarks.push(classification);

        var landmarksStr = JSON.stringify(landmarks);

        if (connectionEstablished && socket.readyState === socket.OPEN) {
          console.log(`Sending ${landmarksStr}`);
          socket.send(landmarksStr);
        } else if (socket.readyState === socket.CLOSED) {
          connect(CONNECT_PORT);
        }
        window.drawConnectors(ctx, landmarks, window.HAND_CONNECTIONS, {color: '#a8dadc', lineWidth: 2});
        window.drawLandmarks(ctx, landmarks, {color: '#f1faee', fillColor: '#a8dadc', radius: 1});
      }
    }
  };

  const hands = new window.Hands({locateFile : (file) => {
    return `https://cdn.jsdelivr.net/npm/@mediapipe/hands/${file}`;
  }});

  var timesTried = 0;
  var timer;
  var cameraSetup = false;
  const initCamera = () => {
    if (
      typeof webcamRef.current != "undefined" &&
      webcamRef.current != null &&
      webcamRef.current.video.readyState === 4 &&
      cameraSetup === false
    ) {
      const camera = new window.Camera(webcamRef.current.video, {
        onFrame: async () => {
          await hands.send({image: webcamRef.current.video});
        },
        width: CAM_WIDTH,
        height: CAM_HEIGHT
      });
      camera.start();
      cameraSetup = true;
      console.log('Camera initalized.');
    }
    console.log('Attempted Camera initialization.');
    timesTried++;
    if (timesTried > 50) {
      clearInterval(timer);
    }
  };

  hands.setOptions({
    maxNumHands: 1,
    minDetectionConfidence: 0.75,
    minTrackingConfidence: 0.5
  });

  console.log('Hands instantiated.');
  hands.onResults( onResult );

  const startCamera = async () => {
    if (typeof hands == "undefined") {
      console.log('Did not set hands.');
    }

    timer = setInterval(() => {
      initCamera(hands);
    }, 100); // originally 100ms
  };

  if (!cameraSetup) {
    startCamera();
  }
  

  return (
    <div className="App">
      <h1 className="App-title">Hand Tracker</h1>
      <header className="App-header">
        <Webcam
          ref={webcamRef}
          style={{
            position: "absolute",
            marginLeft: "auto",
            marginRight: "auto",
            left: 0,
            right: 0,
            textAlign: "center",
            zindex: 9,
            width: CAM_WIDTH,
            height: CAM_HEIGHT,
          }}
        />

        <canvas 
          ref={canvasRef}
          style={{
            position: "absolute",
            marginLeft: "auto",
            marginRight: "auto",
            left: 0,
            right: 0,
            textAlign: "center",
            zindex: 9,
            width: CAM_WIDTH,
            height: CAM_HEIGHT,
          }}
        />
      </header>
    </div>
  );
}

export default App;
