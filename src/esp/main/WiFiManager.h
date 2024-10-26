#ifndef WIFI_MANAGER_H
#define WIFI_MANAGER_H

class WiFiManager 
{
public:
    static void connect();
    static bool isConnected();

private:
    static const char* ssid;
    static const char* password;
};

#endif
