#ifndef WIFI_MANAGER_H
#define WIFI_MANAGER_H

class WiFiManager 
{
public:
    static void connect();
    static bool isConnected();
    static void checkAndReconnect();  // New method for non-blocking reconnection

private:
    static const char* ssid;
    static const char* password;
    static unsigned long lastReconnectAttempt;  // To track last reconnection attempt
    static const unsigned long RECONNECT_INTERVAL = 30000;  // Try to reconnect every 30 seconds
};

#endif