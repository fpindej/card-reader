#include "RemoteHandler.h"
#include "RelayHandler.h"
#include <.env.h>

WebServer RemoteHandler::server(80);
const char* RemoteHandler::loginUsername = REMOTE_USERNAME;
const char* RemoteHandler::loginPassword = REMOTE_PASSWORD;

void RemoteHandler::init() {
    // Serve the login page
    server.on("/", handleRoot); // Root page (login page)
    server.on("/open", handleOpen); // Endpoint to open the door
    server.onNotFound(handleNotFound); // Handle 404 errors

    // Start the server
    server.begin();
    Serial.println("HTTP server started");
}

void RemoteHandler::handleClient() {
    server.handleClient();
}

void RemoteHandler::handleRoot() {
    String html = R"(
        <!DOCTYPE html>
        <html>
        <head>
          <title>Door Control</title>
          <style>
            body { font-family: Arial, sans-serif; text-align: center; margin-top: 50px; }
            button { padding: 10px 20px; font-size: 16px; }
          </style>
        </head>
        <body>
          <h1>Door Control</h1>
          <form action="/open" method="POST">
            <label for="username">Username:</label><br>
            <input type="text" id="username" name="username" required><br><br>
            <label for="password">Password:</label><br>
            <input type="password" id="password" name="password" required><br><br>
            <button type="submit">Open Door</button>
          </form>
        </body>
        </html>
    )";
    server.send(200, "text/html", html);
}

void RemoteHandler::handleOpen() {
    // Check login credentials
    if (server.hasArg("username") && server.hasArg("password")) {
        String username = server.arg("username");
        String password = server.arg("password");

        if (username == loginUsername && password == loginPassword) {
            RelayHandler::activateRelay();
            // Send a success response
            server.send(200, "text/plain", "Door opened!");
        } else {
            // Invalid credentials
            server.send(401, "text/plain", "Unauthorized");
        }
    } else {
        // Missing credentials
        server.send(400, "text/plain", "Bad Request");
    }
}

void RemoteHandler::handleNotFound() {
    server.send(404, "text/plain", "Not Found");
}