const WebSocket = require("ws");

const socket = new WebSocket.Server({ port: 3030 });

socket.on("connection", connection = (ws) => {
    console.log("New client connected.");

    ws.on('message', incoming = (data) => {
        console.log(`Data received`);
        socket.clients.forEach(each = (client) => {
            if (client !== ws && client.readyState === WebSocket.OPEN) {
                client.send(data);
            }
        });
    });

    ws.on("close", closing = () => {
        console.log("Client has disconnected");
    });
});