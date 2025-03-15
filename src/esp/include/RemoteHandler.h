#ifndef REMOTEHANDLER_H
#define REMOTEHANDLER_H

#include <WiFi.h>
#include <WebServer.h>

class RemoteHandler {
public:
    static void init();
    static void handleClient();

private:
    static void handleRoot();
    static void handleOpen();
    static void handleNotFound();

    static WebServer server;
    static const char* loginUsername;
    static const char* loginPassword;
};

#endif